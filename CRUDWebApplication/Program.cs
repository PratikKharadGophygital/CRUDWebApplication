using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}


// Setup Rotativa Converter Html To PDF  exe file.
if (!app.Environment.IsEnvironment("Test") == false)
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