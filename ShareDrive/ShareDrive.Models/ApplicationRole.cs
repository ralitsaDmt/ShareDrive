using Microsoft.AspNetCore.Identity;

namespace ShareDrive.Models
{
    public class ApplicationRole : IdentityRole<int>, IEntity
    {
        public ApplicationRole()
        {

        }

        public ApplicationRole(string name) => this.Name = name;
    }
}
