using ShareDrive.ViewModels.Drive;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IDriveHelperService
    {
        void ProcessCreateDrive(EditViewModel model, int userId);

        void ProcessEditDrive(EditViewModel model, int id);        
    }
}
