using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ResultFilters
{
    public class PersonListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PersonListResultFilter> _logger;

        public PersonListResultFilter(ILogger<PersonListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Before
            _logger.LogInformation("{FilterName}.{MethodName} Method", nameof(PersonListResultFilter), nameof(OnResultExecutionAsync));

            await next(); // call the subsequent filter 

            // After
            _logger.LogInformation("{FilterName}.{MethodName} Method", nameof(PersonListResultFilter), nameof(OnResultExecutionAsync));

            context.HttpContext.Response.Headers["Last-Modify"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
        }
    }
}
