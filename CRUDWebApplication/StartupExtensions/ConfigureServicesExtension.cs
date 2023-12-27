using CRUDWebApplication.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDWebApplication.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services,IConfiguration configuration)
        {
            // EF core connection with sql server by default is services as scoped services 
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

            });

            // Add services to the container. 
            // It add controller and views as services
            services.AddTransient<ResponseHeaderActionFilter>();
            services.AddControllersWithViews(options =>
            {

                //options.Filters.Add<ResponseHeaderActionFilter>();

                // service provider responsible to dispach the service instances
                // GetRequiredService : if logger is not avaliable in service then throw the exception
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                // using this you can pass the argument 
                options.Filters.Add(new ResponseHeaderActionFilter(logger)
                {
                    Key = "My-Key-From-Global",
                    Value = "My-Value-From-Global",
                    Order = 2
                });
            });

            // add services into IOC container 
            // i want create the instace for life time if application is started 
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IContriesRepository, CountriesRepository>();

            services.AddScoped<IPersonService_, PersonService_>();
            services.AddScoped<IPersonsRepository, PersonRepository>();

            services.AddTransient<PersonListActionFilters>();

            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonGetterService, PersonGetterService>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();





            return services;

        }
    }
}
