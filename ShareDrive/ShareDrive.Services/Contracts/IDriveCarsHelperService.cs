using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.CarViewModels;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IDriveCarsHelperService
    {
        DriveCreateEditViewModel GetEditViewModel(int driveId, int userId);

        DriveCreateEditViewModel GetCreateViewModel(int userId);
    }
}
