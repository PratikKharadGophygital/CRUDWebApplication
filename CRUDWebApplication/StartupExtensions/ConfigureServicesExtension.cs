﻿using CRUDWebApplication.Filters.ActionFilters;
using Entities;
using Entities.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDWebApplication.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {


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
            services.AddTransient<PersonListActionFilters>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IContriesRepository, CountriesRepository>();
            //services.AddScoped<IPersonService_, PersonService_>();       
            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonGetterService, PersonGetterService>();
            //services.AddScoped<IPersonGetterService, PersonGetterServiceWithExcelFewField>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();
            services.AddScoped<IPersonsRepository, PersonRepository>();

            // Add the identity Service
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
             {
                 options.Password.RequiredLength = 8;
                // Special chara mandatory or not 
                options.Password.RequireNonAlphanumeric = false;
                 options.Password.RequireUppercase = true;
                 options.Password.RequireLowercase = true;
                 options.Password.RequireDigit = false;
                 options.Password.RequiredUniqueChars = 3; // Eg:Ab12Ab
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            // EF core connection with sql server by default is services as scoped services 
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddAuthorization(options =>
            {
                // enforces authoriation policy (user must be authenticated ) for all the action methods 
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();

                // Custom Authorization Policy
                options.AddPolicy("NotAuthorized", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        // If  IsAuthenticated  return true then operator  convert into false. 
                        // If  IsAuthenticated  return false then operator convert into true.
                        return ! context.User.Identity.IsAuthenticated;
                    });
                });
            });

            // Identity Cookies not found send the usre in login page
            services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = "/Account/Login";
            });

            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

            });


            return services;

        }
    }
}
