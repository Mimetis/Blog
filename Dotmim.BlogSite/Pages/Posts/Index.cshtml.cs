using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dotmim.Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Dotmim.BlogSite.Pages.Posts
{
    public class IndexModel : PageModel
    {
        public HttpClient Client { get; }


        [BindProperty]
        public Post Post { get; set; }
        public Boolean IsNew { get; set; } = false;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            this.Client = httpClientFactory.CreateClient("Posts.Api");

        }

        public async Task<ActionResult> OnGet(string id)
        {

            if (string.IsNullOrEmpty(id))
                return NotFound();

            var url = $"/api/v1/posts/{id}";

            // Get string content
            var downloadedContent = await this.Client.GetStringAsync(url);

            // deserialize to PagedItems
            this.Post = JsonConvert.DeserializeObject<Post>(downloadedContent);

            if (this.Post == null)
                return NotFound();

            return Page();
        }
    }
}