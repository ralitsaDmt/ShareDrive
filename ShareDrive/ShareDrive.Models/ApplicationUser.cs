using Microsoft.AspNetCore.Identity;

namespace ShareDrive.Models
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
