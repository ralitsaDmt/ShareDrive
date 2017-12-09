
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ShareDrive.Helpers
{
    public static class IdentityHelper
    {
        public static int GetUserId(IHttpContextAccessor contextAccessor)
        {
            int userId;
            string userIdAsString = contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int.TryParse(userIdAsString, out userId);

            return userId;
        }
    }
}
