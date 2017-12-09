using ShareDrive.ViewModels.Drive;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IDriveHelperService
    {
        void ProcessCreateDrive(CreateViewModel model, int userId);
    }
}
