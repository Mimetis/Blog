using Dotmim.Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Posts.Api.Infrastructure
{
    class PostTpeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> entity)
        {
            entity.ToTable("Posts");

            entity.HasKey(p=> p.Id);

            entity.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("(newid())");

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.BlogContent)
                .IsRequired();

            entity.Property(p => p.Slug)
                .HasMaxLength(150);

            //entity.HasMany(p => p.Tags)
            //    .WithOne(t => t.Post)
            //    .HasForeignKey(t => t.PostId);

        }
    }
}
