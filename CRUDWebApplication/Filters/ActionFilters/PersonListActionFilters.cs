using CRUDWebApplication.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDWebApplication.Filters.ActionFilters
{
    public class PersonListActionFilters : IActionFilter
    {

        private readonly ILogger<PersonListActionFilters> _logger;

        public PersonListActionFilters(ILogger<PersonListActionFilters> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // To do : add after logic here 
            _logger.LogInformation("{FileName}.{MethodName} Method",nameof(PersonListActionFilters),nameof(OnActionExecuted));
            //throw new NotImplementedException();

            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];
            PersonsController personController = (PersonsController)context.Controller;
            if (parameters != null)
            {
                if (parameters.ContainsKey("serachBy"))
                {
                   
                    personController.ViewData["CurrentSerachBy"] = Convert.ToString(parameters["serachBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {
                    
                    personController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }

                if (parameters.ContainsKey("sortBy"))
                {
                    
                    personController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }

                if (parameters.ContainsKey("sortOrder"))
                {
                    
                    personController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortOrder"]);
                }

                // searching
                personController.ViewBag.SerachFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
                {nameof(PersonResponse.Geneder), "Geneder" },
                {nameof(PersonResponse.CountryID), "CountryID" },
                {nameof(PersonResponse.Address), "Address" },
            };
            }
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // To do : add before logic here
           
            _logger.LogInformation("{FileName}.{MethodName} Method", nameof(PersonListActionFilters), nameof(OnActionExecuting));

            context.HttpContext.Items["arguments"]= context.ActionArguments;
            // Validate the searchBy parameter value 
            if (context.ActionArguments.ContainsKey("serachBy"))
            {
                string? serachBy = Convert.ToString(context.ActionArguments["serachBy"]);

                if (!string.IsNullOrEmpty(serachBy))
                {
                    var serachByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Geneder),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                        
                    };

                    // Reset the searchBy parameter value 
                    if(serachByOptions.Any(temp => temp == serachBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", serachBy);
                        context.ActionArguments["serachBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy update value {searchBy}", serachBy);
                    }
                }
            }
        }
    }
}
