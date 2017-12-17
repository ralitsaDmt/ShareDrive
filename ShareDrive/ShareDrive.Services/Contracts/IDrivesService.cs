using ShareDrive.Models;
using ShareDrive.Services.Models;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface IDrivesService
    {
        void Update(DriveCreateEditViewModel model, ParsedDriveUpdateData data, int id);

        Task Create(DriveCreateEditViewModel model, ParsedDriveCreateData data);

        IEnumerable<DriveIndexViewModel> GetAll();

        // List<DriveIndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null);

        IEnumerable<ShareDrive.ViewModels.Admin.Drive.DriveAdminIndexViewModel> GetAllAdmin();

        ShareDrive.ViewModels.Admin.Drive.DriveAdminDetailsViewModel GetDetailsAdminModel(int id);

        Task CreateAsync(DriveCreateEditViewModel model, City cityFrom, City cityTo, int userId);

        

        DriveCreateEditViewModel GetEditModelById(int id);

        DriveDeleteViewModel GetDeleteModelById(int id);

        Drive GetById(int id);

        bool Delete(int id);

        DriveCollectionsViewModel GetDetailsModel(int id, int userId);

        KeyValuePair<bool, string> ReserveSeat(int driveId, int userId);

        KeyValuePair<bool, string> CancelReservation(int driveId, int userId);
    }
}
