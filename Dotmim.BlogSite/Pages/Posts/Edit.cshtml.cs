using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dotmim.Common.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Dotmim.BlogSite.Pages.Posts
{
    [Authorize]
    public class EditModel : PageModel
    {

        public HttpClient Client { get; }


        [BindProperty]
        public Post Post { get; set; }
        public Boolean IsNew { get; set; } = false;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            this.Client = httpClientFactory.CreateClient("Posts.Api");

        }
        public async Task OnGet(Guid? id)
        {

            if (!id.HasValue)
            {
                this.Post = new Post
                {
                    Id = Guid.NewGuid(),
                    LastModified = DateTime.Now
                };

                this.IsNew = true;
            }
            else
            {
                var url = $"/api/v1/posts/edit/{id.Value.ToString()}";

                // Get string content
                var downloadedContent = await this.Client.GetStringAsync(url);

                // deserialize to PagedItems
                this.Post = JsonConvert.DeserializeObject<Post>(downloadedContent);
            }
        }


        public async Task OnPost()
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(this.Post.Slug))
                    this.Post.Slug = Post.CreateSlug(this.Post.Title);

                if (this.Post.IsPublished && this.Post.PubDate == null)
                    this.Post.PubDate = DateTime.Now;
                else if (!this.Post.IsPublished)
                    this.Post.PubDate = null;

                this.Post.LastModified = DateTime.Now;

                var url = $"/api/v1/posts/edit";
                var response = await this.Client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(this.Post)));
                var downloadedContent = await response.Content.ReadAsStringAsync();
                this.Post = JsonConvert.DeserializeObject<Post>(downloadedContent);
            }


        }


    }
}