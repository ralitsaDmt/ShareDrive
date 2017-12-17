using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.Drive;
using System.Collections.Generic;

namespace ShareDrive.Services
{
    public class DriveCarsHelperService : IDriveCarsHelperService
    {
        private readonly ICarsService carsService;
        private readonly IDrivesService drivesService;

        public DriveCarsHelperService(ICarsService carsService, IDrivesService drivesService)
        {
            this.carsService = carsService;
            this.drivesService = drivesService;
        }

        public DriveCreateEditViewModel GetCreateViewModel(int userId)
        {
            List<CarSelectViewModel> cars = this.GetCarsSelectionList(userId);
            DriveCreateEditViewModel model = new DriveCreateEditViewModel
            {
                Cars = cars
            };
            return model;
        }

        public DriveCreateEditViewModel GetEditViewModel(int driveId, int userId)
        {
            List<CarSelectViewModel> cars = this.GetCarsSelectionList(userId);

            DriveCreateEditViewModel model = this.drivesService.GetEditModelById(driveId);

            model.Cars = cars;

            return model;
        }

        private List<CarSelectViewModel> GetCarsSelectionList(int userId)
        {
            List<CarSelectViewModel> carsSelect = this.carsService.GetSelectionListByDriver(userId);

            if (carsSelect.Count > 1)
            {
                carsSelect.Insert(0, new CarSelectViewModel(0, "Select"));
            }

            return carsSelect;
        }
    }
}
