using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dotmim.Common.Extensions
{
    public static class HostExtensions
    {

        /// <summary>
        /// Create, Migrate and Seed a database, after starting host.
        /// Call this method in Program Main, after creating your IWebHost instance
        /// </summary>
        public static void SeedDatabase<T>(this IWebHost host) where T : DbContext
        {
            // Create a scope to get a DI service (EF context)
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Getting the blog context injected service
                var context = services.GetRequiredService<T>();

                // Adding a retry policy, 3 times, each times space by 3 sec (3, 6, 9 sec)
                // Just in case the docker container is not created
                var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(3, r => TimeSpan.FromSeconds(r * 3));

                // Apply any migration
                retryPolicy.Execute(() => context.Database.Migrate());
            }
        }
    }
}
