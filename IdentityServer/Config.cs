// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer
{
    public static class Config
    {

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        /// <summary>
        /// Get all Apis to protect
        /// </summary>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[] {
                new ApiResource("postsapi", "Posts.Api")
             };
        }


        /// <summary>
        /// Get all clients, ie all web apps, mobile apps, desktop apps, server processes, allowed to access 
        /// </summary>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "dotmim",
                    ClientName = "Dotmim Hybrid Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequireConsent = true,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris           =
                    {
                        "https://localhost:44303/signin-oidc"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44303/signout-callback-oidc",
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "postsapi"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}