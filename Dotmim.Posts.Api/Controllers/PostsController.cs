using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dotmim.Common;
using Dotmim.Common.Infrastructure;
using Dotmim.Common.Model;
using Dotmim.Common.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotmim.Posts.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        public BlogContext Context { get; }

        public PostsController(BlogContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));

            // No need to track change here
           // this.Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        // GET api/v1/Posts[?pageSize=3&pageIndex=10]
        [HttpGet]
        [ProducesResponseType(typeof(PagedItems<Post>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ItemsAsync([FromQuery]int pageSize = 10, [FromQuery]int pageIndex = 0)
        {
            // Get a count of all posts
            var totalItems = await this.Context.Posts.LongCountAsync();

            // Get paged items
            var itemsOnPage = await this.Context.Posts
                .OrderByDescending(c => c.PubDate)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToListAsync();

            var items = new PagedItems<Post>(pageIndex, pageSize, totalItems, itemsOnPage);

            return new JsonResult(items);
        }

        [HttpGet]
        [Route("{slug:minlength(1)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Post>> ItemByIdAsync(string slug)
        {
            var item = await this.Context.Posts.FirstOrDefaultAsync(p => p.Slug == slug);

            if (item != null)
                return item;

            return NotFound();
        }

        [HttpGet]
        [Route("edit/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Post>> Item(string id)
        {
            //var postId = Guid.Empty;

            if (!Guid.TryParse(id, out var postId))
                return BadRequest($"{id} is not a correct post id");

            var item = await this.Context.Posts.FirstOrDefaultAsync(p => p.Id == postId);

            if (item != null)
                return item;

            return NotFound();
        }

        [HttpPost]
        [Route("edit")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Post), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Post>> Save(Post post)
        {
            Post item;
            item = await this.Context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);

            if (item == null)
            {
                item = new Post();
                this.Context.Posts.Add(item);
            }

            item.BlogContent = post.BlogContent;
            item.Excerpt = post.Excerpt;
            item.IsPublished = post.IsPublished;
            item.LastModified = post.LastModified;
            item.PubDate = post.PubDate;
            item.Slug = post.Slug;
            item.Tags = post.Tags;
            item.Title = post.Title;
            await this.Context.SaveChangesAsync();

            return item;
        }
    }
}
