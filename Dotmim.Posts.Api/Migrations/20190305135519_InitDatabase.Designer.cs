﻿// <auto-generated />
using System;
using Dotmim.Common;
using Dotmim.Common.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dotmim.Posts.Api.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20190305135519_InitDatabase")]
    partial class InitDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Dotmim.Common.Model.Comment", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("CommentContent");

                    b.Property<string>("Email");

                    b.Property<bool?>("IsAdmin");

                    b.Property<Guid>("PostID");

                    b.Property<DateTime?>("PubDate");

                    b.HasKey("ID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Dotmim.Common.Model.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("BlogContent")
                        .IsRequired();

                    b.Property<string>("Excerpt");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTime?>("LastModified");

                    b.Property<DateTime?>("PubDate");

                    b.Property<string>("Slug")
                        .HasMaxLength(150);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Posts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"),
                            BlogContent = "Welcome !",
                            IsPublished = true,
                            LastModified = new DateTime(2019, 3, 5, 14, 55, 19, 356, DateTimeKind.Local).AddTicks(2731),
                            PubDate = new DateTime(2019, 3, 5, 14, 55, 19, 355, DateTimeKind.Local).AddTicks(902),
                            Title = "Welcome"
                        });
                });

            modelBuilder.Entity("Dotmim.Common.Model.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("PostId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PostId = new Guid("b81ba09f-6ca7-45c3-8319-1f002ae36c49"),
                            Title = "#Welcome"
                        });
                });

            modelBuilder.Entity("Dotmim.Common.Model.Tag", b =>
                {
                    b.HasOne("Dotmim.Common.Model.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}