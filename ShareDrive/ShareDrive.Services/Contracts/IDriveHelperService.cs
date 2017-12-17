using ShareDrive.ViewModels.Drive;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface IDriveHelperService
    {
        Task ProcessCreateDriveAsync(DriveCreateEditViewModel model, int userId);

        Task ProcessEditDriveAsync(DriveCreateEditViewModel model, int id);

        DriveCreateEditViewModel GetEditViewModel(int driveId, int userId);

        DriveCreateEditViewModel GetCreateViewModel(int userId);
    }
}
