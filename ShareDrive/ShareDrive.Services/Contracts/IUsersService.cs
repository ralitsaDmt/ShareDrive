using ShareDrive.ViewModels.Admin.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IUsersService
    {
        IEnumerable<IndexViewModel> GetAllAdmin();

        DetailsViewModel GetDetailsModel(int id);

        bool Delete(int id);
    }
}
