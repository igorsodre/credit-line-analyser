using CreditLineAnalyser.Web.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CreditLineAnalyser.Web.Filters;

public class ThrottlingFilter : IAsyncActionFilter
{
    private readonly IThrottleService _throttleService;

    public ThrottlingFilter(IThrottleService throttleService)
    {
        _throttleService = throttleService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result = await _throttleService.GetResponse();
        if (result.ShouldThrottle)
        {
            context.Result = result.Response;
            return;
        }

        await next();
    }
}
