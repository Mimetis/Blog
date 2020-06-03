using Dotmim.Common.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Posts.Api.Infrastructure
{
    class TagTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> entity)
        {
            entity.ToTable("Tags");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).UseIdentityColumn();
            entity.Property(c => c.Title).IsRequired().HasMaxLength(150);

            entity.HasOne(t => t.Post)
                .WithMany(p => p.Tags)
                .HasForeignKey(t => t.PostId);

        }
    }
}
