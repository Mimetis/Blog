using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dotmim.Common.Model;
using Dotmim.Common.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Dotmim.BlogSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IUrlHelper urlHelperFactory;
        private readonly IRouteCollection routeCollection;
        private readonly IRouteHandler routeHandler;
        private readonly IRouter router;

        public HttpClient Client { get; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            this.Client = httpClientFactory.CreateClient("Posts.Api");

        }


        public override PageResult Page()
        {
            return base.Page();
        }
        /// <summary>
        /// List of Posts for a current page
        /// </summary>
        public PagedItems<Post> Posts { get; private set; }

        public async Task OnGet([FromRoute]int page = 0)
        {
            try
            {
                ViewData["ActiveLink"] = "Home";

                var url = "/api/v1/posts?pageSize=10&pageIndex={0}";
                var postsUrl = string.Format(url, 10 * page);

                // Get string content
                // var downloadedContent = await this.Client.GetStringAsync(postsUrl);
                //// deserialize to PagedItems
                // this.Posts = JsonConvert.DeserializeObject<PagedItems<Post>>(downloadedContent);

                string d = @"{" +
                "  \"PageIndex\": 0," +
                "  \"PageSize\": 10," +
                "  \"Count\": 1," +
                "  \"Data\": [" +
                "    {" +
                "      \"Id\": \"53430b5c-d705-4984-88c6-7131de866b1a\"," +
                "      \"Title\": \"Welcome\"," +
                "      \"Slug\": \"welcome\"," +
                "      \"Excerpt\": null," +
                "      \"BlogContent\": \"Welcome !\"," +
                "      \"PubDate\": \"2019-03-05T16:43:08.7418388\"," +
                "      \"LastModified\": \"2019-03-05T16:43:08.7430778\"," +
                "      \"IsPublished\": true," +
                "      \"Tags\": []" +
                "    }" +
                "  ]" +
                "}";

                this.Posts = JsonConvert.DeserializeObject<PagedItems<Post>>(d);

                var t = await Task.FromResult(true);


                //var posts = new List<Post>();
                //var welcomePost = new Post { Id = Guid.NewGuid(), Title = "Welcome", Slug = Post.CreateSlug("Welcome"), BlogContent = "Welcome !", IsPublished = true, PubDate = DateTime.Now, LastModified = DateTime.Now };
                //var welcomeTag = new Tag { Id = 1, Title = "#Welcome", PostId = welcomePost.Id };
                //posts.Add(welcomePost);
                //var pageItems = new PagedItems<Post>(0, 10, 1, posts);
                //var ser = JsonConvert.SerializeObject(pageItems);
                //var d = JsonConvert.DeserializeObject<PagedItems<Post>>(ser);

                // get the page count
                var totalPage = Math.Ceiling((decimal)this.Posts.Count / (decimal)this.Posts.PageSize);
                // generate next and prev
                var next = this.Posts.PageIndex >= totalPage - 1 ? string.Empty : $"/{this.Posts.PageIndex + 1}/";
                var prev = this.Posts.PageIndex <= 0 ? string.Empty : this.Posts.PageIndex >= totalPage - 1 ? string.Empty : $"/{this.Posts.PageIndex + 1}/";

                ViewData["next"] = next != string.Empty ? string.Format(url, next) : string.Empty;
                ViewData["prev"] = prev != string.Empty ? string.Format(url, prev) : string.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
