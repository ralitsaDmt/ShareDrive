using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace ShareDrive.Infrastructure.Filters
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request?.Headers
                        ["X-Requested-With"].ToString() != "XMLHttpRequest")
            {
                context.Result = new RedirectToRouteResult(
    new RouteValueDictionary(new { controller = "Home", action = "Error" })
);
            }
        }
    }
}
