using ShareDrive.Models;
using ShareDrive.Services.Models;
using ShareDrive.ViewModels.Admin.Drive;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface IDrivesService
    {
        IEnumerable<DriveIndexViewModel> GetAll();

        IEnumerable<DriveAdminIndexViewModel> GetAllAdmin();

        Drive GetById(int id);

        Task CreateAsync(DriveCreateEditViewModel model, ParsedDriveCreateData data);

        DriveCreateEditViewModel GetEditModelById(int id);
        
        Task<bool> UpdateAsync(DriveCreateEditViewModel model, ParsedDriveUpdateData data, int id);

        DriveDeleteViewModel GetDeleteModelById(int id);

        bool Delete(int id);

        // List<DriveIndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null);


        ShareDrive.ViewModels.Admin.Drive.DriveAdminDetailsViewModel GetDetailsAdminModel(int id);
        
        

        

        

        

        

        DriveCollectionsViewModel GetDetailsModel(int id, int userId);

        KeyValuePair<bool, string> ReserveSeat(int driveId, int userId);

        KeyValuePair<bool, string> CancelReservation(int driveId, int userId);
    }
}
