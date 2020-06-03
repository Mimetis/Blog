using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Dotmim.IdentityServer.Areas.Identity.Pages.Consent
{
    [Authorize]
    //[SecurityHeaders]
    public class IndexModel : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            ILogger<IndexModel> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore ?? throw new ArgumentNullException(nameof(resourceStore));
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public ConsentInputModel Input { get; set; }

        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            await BuildViewModelAsync(returnUrl);

            if (this.Input == null)
            {
                this.StatusMessage = "No consent to grant";
            }

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string button)
        {
            var result = await ProcessConsent(button);

            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            //if (result.ShowView)
            //{
            //    return Page();
            //}

            throw new ApplicationException($"Error on consent validation");
        }


        private async Task<ProcessConsentResult> ProcessConsent(string button)
        {
            var result = new ProcessConsentResult();

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (button == "no")
            {
                grantedConsent = ConsentResponse.Denied;
            }
            // user clicked 'yes' - validate the data
            else if (button == "yes" && Input != null)
            {
                // if the user consented to some scope, build the response Input
                List<string> scopes = new List<string>();
                if (Input.IdentityScopes != null && Input.IdentityScopes.Any())
                    scopes.AddRange(Input.IdentityScopes.Where(isc => isc.Checked).Select(isc => isc.Name).ToList());

                if (Input.ResourceScopes != null && Input.ResourceScopes.Any())
                    scopes.AddRange(Input.ResourceScopes.Where(isc => isc.Checked).Select(isc => isc.Name).ToList());

                if (scopes.Count > 0 && !ConsentOptions.EnableOfflineAccess)
                    scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess).ToList();

                if (scopes.Count > 0)
                {
                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = Input.RememberConsent,
                        ScopesConsented = scopes
                    };
                }
                else
                {
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                // validate return url is still valid
                var request = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);
                if (request == null) return result;

                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = Input.ReturnUrl;
            }
            else
            {
                // we need to redisplay the consent UI
                await BuildViewModelAsync(Input.ReturnUrl);
            }

            return result;
        }


        private async Task BuildViewModelAsync(string returnUrl)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (request == null)
            {
                var message = $"Error: No consent request matching request for returnUrl {returnUrl}";
                this.StatusMessage = message;
                this._logger.LogError(message);
                return;
            }
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);

            if (client == null)
            {
                var message = $"Error: Invalid client id: {request.ClientId}";
                this.StatusMessage = message;
                this._logger.LogError(message);
                return;
            }

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            var scopesExisting = resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any());

            if (!scopesExisting)
            {
                var message = $"No scopes matching: {request.ScopesRequested.Aggregate((x, y) => x + ", " + y)}";
                this.StatusMessage = message;
                this._logger.LogError(message);
                return;
            }

            CreateConsentViewModel(returnUrl, request, client, resources);
        }



        private void CreateConsentViewModel(string returnUrl,
            AuthorizationRequest request,
            Client client, Resources resources)
        {

            // on First load, create an object instance
            // otherwise, created by asp.net mvc form
            if (Input == null) Input = new ConsentInputModel();

            this.ClientName = client.ClientName ?? client.ClientId;
            this.ClientUrl = client.ClientUri;
            this.ClientLogoUrl = client.LogoUri;
            this.AllowRememberConsent = client.AllowRememberConsent;

            Input.RememberConsent = true;
            Input.ReturnUrl = returnUrl;

            Input.IdentityScopes = resources.IdentityResources
                                   .Select(x => CreateScopeViewModel(x)).ToList();

            Input.ResourceScopes = resources.ApiResources
                                   .SelectMany(x => x.Scopes)
                                   .Select(x => CreateScopeViewModel(x)).ToList();

            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                if (Input.ResourceScopes == null)
                    Input.ResourceScopes = new List<ScopeViewModel>();

                var offlineScope = GetOfflineAccessScope(true);
                var scopeviemodels = new ScopeViewModel[] { offlineScope };

                Input.ResourceScopes = Input.ResourceScopes.Union(scopeviemodels).ToList();
            }

        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check = true)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check = true)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }
    }

    public class ConsentInputModel
    {
        private List<ScopeViewModel> _identityScopes;

        public List<string> ScopesConsented { get; set; } = new List<string>();
        public bool RememberConsent { get; set; }
        public string ReturnUrl { get; set; }

        public List<ScopeViewModel> IdentityScopes
        {
            get => _identityScopes;
            set => _identityScopes = value;
        }
        public List<ScopeViewModel> ResourceScopes { get; set; }

    }

    public class ScopeViewModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }
    }

    public class ConsentOptions
    {
        public static bool EnableOfflineAccess = true;
        public static string OfflineAccessDisplayName = "Offline Access";
        public static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

        public static readonly string MustChooseOneErrorMessage = "You must pick at least one permission";
        public static readonly string InvalidSelectionErrorMessage = "Invalid selection";
    }

    public class ProcessConsentResult
    {
        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri { get; set; }

        //public bool ShowView => ViewModel != null;
        //public ConsentViewModel ViewModel { get; set; }

        public bool HasValidationError => ValidationError != null;
        public string ValidationError { get; set; }
    }
}