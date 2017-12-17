using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ShareDrive.Infrastructure.Filters
{
    public class AjaxAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //if (context.HttpContext.Request.Headers.)
        }
    }
}
