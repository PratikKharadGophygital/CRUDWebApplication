using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDWebApplication.Filters.ResultFilters
{
    /// <summary>
    /// IAlwaysRunResultFilter : Always work when short curcuit and result as 
    /// </summary>
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }
    }
}
