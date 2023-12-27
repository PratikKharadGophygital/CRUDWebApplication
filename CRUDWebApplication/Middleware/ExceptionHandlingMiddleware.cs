using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Threading.Tasks;

namespace CRUDWebApplication.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IDiagnosticContext _diagnosticContext; 

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IDiagnosticContext diagnosticContext)
        {
            _next = next;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                // It call the next middle which one is executed.
               await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    _logger.LogError("{ExceptionType},{ExceptionMessage}",ex.InnerException.GetType().ToString(),ex.InnerException.Message);
                }
                else
                {
                    _logger.LogError("{ExceptionType},{ExceptionMessage}", ex.GetType().ToString(), ex.Message);
                }

                //  Custom exception error message 
                //httpContext.Response.StatusCode = 500;
                //httpContext.Response.WriteAsync("Internal Server Error Occurred. Please Contact Admin");

                // This statement called the build in exception handle middlerware 
                // Same exception object is rethrown from here 
                throw;
            }
           
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
