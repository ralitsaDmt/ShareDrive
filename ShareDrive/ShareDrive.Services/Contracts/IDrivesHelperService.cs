using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface IDrivesHelperService
    {
        Task ProcessCreateDriveAsync(DriveCreateEditViewModel model, int userId);

        Task ProcessEditDriveAsync(DriveCreateEditViewModel model, int id);

        DriveCreateEditViewModel GetEditViewModel(int driveId, int userId);

        DriveCreateEditViewModel GetCreateViewModel(int userId);
    }
}
