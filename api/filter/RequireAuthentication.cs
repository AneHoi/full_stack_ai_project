using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.filter;

public class RequireAuthentication: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //If sessionData is empty, we cannot let anyone into, where this filter is present
        if (context.HttpContext.GetSessionData() == null) throw new AuthenticationException();
    }
}