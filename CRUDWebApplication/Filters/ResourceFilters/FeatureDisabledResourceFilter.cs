using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool _isDisable;

        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisable = true)
        {
            _logger = logger;
            _isDisable = isDisable;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            // Before
            _logger.LogInformation("{FilterName}.{MethodName} Method", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));

            if (_isDisable)
            {
                // context.Result = new NotFoundResult(); // 404 - Not Found 
                context.Result = new StatusCodeResult(501); // 501 - Not Implement 
            }
            else
            {
                await next();
            }



            // After
            _logger.LogInformation("{FilterName}.{MethodName} Method", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
        }
    }
}
