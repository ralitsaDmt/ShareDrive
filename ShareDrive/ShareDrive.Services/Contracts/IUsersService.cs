using ShareDrive.Models;
using ShareDrive.ViewModels.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IUsersService
    {
        IEnumerable<UserAdminIndexViewModel> GetAllAdmin();

        UserAdminDetailsViewModel GetDetailsModel(int id);

        bool Delete(int id);

        ApplicationUser GetById(int id);
    }
}
