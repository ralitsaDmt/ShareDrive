namespace ShareDrive.Services.Contracts
{
    using System.Collections.Generic;

    using ShareDrive.Models;
    using ShareDrive.ViewModels.Admin.User;

    public interface IUsersService
    {
        bool CheckIfUserExists(int userId);

        ApplicationUser GetById(int id);

        IEnumerable<UserAdminIndexViewModel> GetAllAdmin();

        UserAdminDetailsViewModel GetDetailsModel(int id);

        bool Delete(int id);
    }
}
