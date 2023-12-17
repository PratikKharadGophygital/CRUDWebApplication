using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ActionFilters
{
    public class ResponseHeaderActionFilter : IAsyncActionFilter,IOrderedFilter
    {

        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        private readonly string _key;
        private readonly string _value;

        // Read only property 
        public int Order { get; set; }

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger,string Key,string Value,int order)
        {
            _logger = logger;
            _key = Key;
            _value = Value;
            Order = order;
        }

        // before 
        //public void OnActionExecuting(ActionExecutingContext context)
        //{

        //}

        // after
        //public void OnActionExecuted(ActionExecutedContext context)
        //{
            

            
        //}

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Execution Before
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            // Must be add the this method is required for working 
            await next();

            // Execution After
            _logger.LogInformation("{FilterName}.{MethodName}", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));

            context.HttpContext.Response.Headers[_key] = _value;
        }
    }
}
