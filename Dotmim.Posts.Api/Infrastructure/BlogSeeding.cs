using Dotmim.Common.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Posts.Api.Infrastructure
{
    public static class BlogSeeding
    {
        public static void ApplySeed(this ModelBuilder modelBuilder)
        {
            var welcomePost = new Post { Id = Guid.NewGuid(), Title = "Welcome", Slug = Post.CreateSlug("Welcome"), BlogContent = "Welcome !", IsPublished = true, PubDate = DateTime.Now, LastModified = DateTime.Now };
            
            var welcomeTag = new Tag { Id = 1, Title = "#Welcome", PostId= welcomePost.Id };

            modelBuilder.Entity<Post>().HasData(welcomePost);
            modelBuilder.Entity<Tag>().HasData(welcomeTag);
        }
    }
}
