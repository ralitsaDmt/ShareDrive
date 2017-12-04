using ShareDrive.Common;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using ShareDrive.Infrastructure;

namespace ShareDrive.Services
{
    public class RolesService : IRolesService
    {
        private readonly IDbRepository<ApplicationRole> roles;

        public RolesService(IDbRepository<ApplicationRole> roles)
        {
            this.roles = roles;
        }

        public IEnumerable<ApplicationRole> GetUserRoles()
        {
            return this.roles.GetAll()
                .Where(x => x.Name != GlobalConstants.RoleAdministrator)
                .ToList();
        }
    }
}
