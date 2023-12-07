using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using Serilog;
var builder = WebApplication.CreateBuilder(args);


// Add the Logging method 
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider.ClearProviders();
//    loggingProvider.AddConsole();
//    loggingProvider.AddDebug();
//    loggingProvider.AddEventLog();
//});

//
builder.Host.UseSerilog(HttpBuilderContext context,IServiceProvider services, LoggerConfiguration loggerConfiguration =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // Read configuration settings from built in IConfiguration
    .ReadFrom.Services(services); // Read out current apps services and make them available to serilog
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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