using ShareDrive.Models;
using System.Collections.Generic;

namespace ShareDrive.Services.Contracts
{
    public interface IRolesService
    {

        IEnumerable<ApplicationRole> GetUserRoles();
    }
}
