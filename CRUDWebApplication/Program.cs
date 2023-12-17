using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
using CRUDWebApplication.Filters.ActionFilters;

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
builder.Host.UseSerilog((HostBuilderContext context,IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // Read configuration settings from built in IConfiguration
    .ReadFrom.Services(services); // Read out current apps services and make them available to serilog
});

// Add services to the container. 
// It add controller and views as services
builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>();

    // service provider responsible to dispach the service instances
    // GetRequiredService : if logger is not avaliable in service then throw the exception
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
    // using this you can pass the argument 
    options.Filters.Add(new ResponseHeaderActionFilter(logger, "My-Key-From-Global", "My-Value-From-Global",2));
});

// add services into IOC container 
// i want create the instace for life time if application is started 
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IContriesRepository, CountriesRepository>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonsRepository, PersonRepository>();

// EF core connection with sql server by default is services as scoped services 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
    Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;

});

var app = builder.Build();

// Enable endpoint completion log that means adds ab extra log message as soon as the request response is completed
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

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

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Create for integration testing 
public partial class Program { }