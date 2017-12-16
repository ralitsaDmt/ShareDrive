using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IDrivesService
    {
        List<IndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null);

        IEnumerable<ShareDrive.ViewModels.Admin.Drive.IndexViewModel> GetAllAdmin();

        ShareDrive.ViewModels.Admin.Drive.DetailsViewModel GetDetailsAdminModel(int id);

        void Create(EditViewModel model, City cityFrom, City cityTo, int userId);

        void Update(EditViewModel model, City cityFrom, City cityTo, int id);

        EditViewModel GetEditModelById(int id);

        DeleteViewModel GetDeleteModelById(int id);

        Drive GetById(int id);

        bool Delete(int id);

        DetailsViewModel GetDetailsModel(int id, int userId);

        KeyValuePair<bool, string> ReserveSeat(int driveId, int userId);

        KeyValuePair<bool, string> CancelReservation(int driveId, int userId);
    }
}
