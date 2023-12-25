using CRUDWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDWebApplication.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // To Do : Before
            if (context.Controller is PersonsController personsController) // check runtime type 
            {
                if (!personsController.ModelState.IsValid)
                {
                    var personRequest = context.ActionArguments["personRequest"];
                    List<CountryResponse> countries = await _countriesService.GetAllCountryList();
                    personsController.ViewBag.Countries = countries;
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                    context.Result = personsController.View(personRequest); // Short-circuits or skips the subsequent action filters & action method 
                }
            }

            await next();
            // To Do : After
        }
    }
}
