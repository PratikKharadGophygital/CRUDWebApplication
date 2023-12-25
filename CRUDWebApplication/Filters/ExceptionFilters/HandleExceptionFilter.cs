using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment; // which enviroment run application
        private readonly ILogger<HandleExceptionFilter> _logger;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Only exception is found in model binding and both action filter and method then exception filter is execute other wise eception filter not execute 
        /// </summary>        
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);


            if (_hostEnvironment.IsDevelopment())
            {
                context.Result = new ContentResult()
                {
                    Content = context.Exception.Message,
                    StatusCode = 500
                };
            }

        }
    }
}
