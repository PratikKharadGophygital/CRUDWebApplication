using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace TestProject1
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // descripter -> represent the type of the service and lifetime 
                var descripter = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if(descripter != null)
                {
                    // Remove the service 
                    services.Remove(descripter);
                }

                services.AddDbContext<ApplicationDbContext>( options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting");
                });
            });
        }
    }
}
