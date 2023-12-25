using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        // If value is true => Filter object instance can be accessible across mutiple rquest application 
        public bool IsReusable => false;

        private string? Key { get; set; }
        private string? Value { get; set; }
        private int Order { get; set; }

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }

        // Controller -> FilterFactory -> Filter 
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order;
            return filter;
        }
    }


    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    {

        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        //private readonly string _key;
        //private readonly string _value;

        public string? Key { get; set; }
        public string? Value { get; set; }
        // Read only property 
        public int Order { get; set; }

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;

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

            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
