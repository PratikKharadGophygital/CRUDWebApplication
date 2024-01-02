using CRUDWebApplication.Filters.ActionFilters;
using CRUDWebApplication.Middleware;
using CRUDWebApplication.StartupExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add the Logging method 
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddDebug();
//    loggingProvider.AddEventLog();
//});

// Serilog is enable 
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // Read configuration settings from built in IConfiguration
    .ReadFrom.Services(services); // Read out current apps services and make them available to serilog
});



//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(Configuration..GetConnectionString("DefaultConnection"));
//});
// Using Extension Method for services 
// Using parameter send the value of builder.Configuration for extension method because extension method does not access the builder services that the reason we are pass the value of the parameter 
builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline. 
// When env is development then show the exception page.
if (builder.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/home/error");
    // Only for when app is production stage 
    app.UseExceptionHandlingMiddleware();
}

// Enable endpoint completion log that means adds ab extra log message as soon as the request response is completed
app.UseSerilogRequestLogging();

//if (builder.Environment.IsProduction())
//{
//    app.UseExceptionHandler("/Home/Error");

//}

// Enable the Http rquest Logging Track every rquest log.
app.UseHttpLogging();

//app.Logger.LogDebug("LogDebug-Message");
//app.Logger.LogInformation("LogInformation-Message");
//app.Logger.LogWarning("LogWarning-Message");
//app.Logger.LogError("LogError-Message");
//app.Logger.LogCritical("LogCritical-Message");


// Setup Rotativa Converter Html To PDF  exe file.
if (builder.Environment.IsEnvironment("Test") == false)
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}


app.UseStaticFiles();



// Identitying action method based route
app.UseRouting();

// Reading Identity cookie and check user login or not 
app.UseAuthentication();

// Check User have access or not praticular resources
app.UseAuthorization();

// Execute the filter pipiline (action + filters) 
app.MapControllers();

// This is default routing and conventional routing now days not use this  so if you declare here then dont need to declare the controller
// This is the global conventional routing 
app.UseEndpoints(endpoints =>
{
    // This is the conventional rounting for the 'Area'
    endpoints.MapControllerRoute(
        name:"areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}"
        // Admin/Home/Index
        );

    // Default for genral users
    endpoints.MapControllerRoute(
        name:"default",
        pattern:"{controller}/{action}/{Id?}"
        );
});
app.Run();

// Create for integration testing 
public partial class Program { }