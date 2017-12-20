namespace ShareDrive.Services
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;

    using AutoMapper;

    using ShareDrive.Services.Contracts;
    using ShareDrive.ViewModels.Drive;
    using System.Threading.Tasks;
    using ShareDrive.Models;
    using ShareDrive.Services.Models;
    using ShareDrive.ViewModels.Car;
    using ShareDrive.Core;
    using ShareDrive.Exceptions;

    public class DriveHelperService : IDriveHelperService
    {
        private readonly IDrivesService drivesService;
        private readonly ICitiesService citiesService;
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;
        private readonly IMapper mapper;

        public DriveHelperService(IDrivesService drivesService,
            ICitiesService citiesService, ICarsService carsService,
            IUsersService usersService, IMapper mapper)
        {
            this.drivesService = drivesService;
            this.citiesService = citiesService;
            this.carsService = carsService;
            this.usersService = usersService;
            this.mapper = mapper;
        }

        public DriveCreateEditViewModel GetCreateViewModel(int userId)
        {
            Require.ThatIntIsPositive(userId);

            List<CarSelectViewModel> cars = this.GetCarsSelectionList(userId);

            DriveCreateEditViewModel model = new DriveCreateEditViewModel
            {
                Cars = cars
            };
            return model;
        }

        public async Task ProcessCreateDriveAsync(DriveCreateEditViewModel model, int userId)
        {
            Require.ThatObjectIsNotNull(model);
            Require.ThatIntIsPositive(userId);

            var parsedData = await this.ParseModelDataCreate(model.From, model.To, model.CarId, model.DateTime, userId);
            await this.drivesService.CreateAsync(model, parsedData);
        }

        public DriveCreateEditViewModel GetEditViewModel(int driveId, int userId)
        {
            // test - ok
            Require.ThatIntIsPositive(driveId);
            // test - ok
            Require.ThatIntIsPositive(userId);

            // test with non exising driveId => returns null - ok
            DriveCreateEditViewModel model = this.drivesService.GetEditModelById(driveId);

            if (model != null)
            {
                // test with non existing userId => returns empty list
                // test with existing userId => returns valid model
                List<CarSelectViewModel> cars = this.GetCarsSelectionList(userId);
                model.Cars = cars;
                return model;
            }

            return null;
        }

        public async Task ProcessEditDriveAsync(DriveCreateEditViewModel model, int driveId)
        {
            // test
            Require.ThatObjectIsNotNull(model);
            // test
            Require.ThatIntIsPositive(driveId);
            
            // test with invalid date format => should throw InvalidDateFormatExcepton
            var parsedData = await this.ParseModelData(model.From, model.To, model.CarId, model.DateTime);

            // test with valid model, data and invalid driveId => return false
            // test with valid model, data and drive id => return true
            this.drivesService.UpdateAsync(model, parsedData, driveId);
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

        private async Task<ParsedDriveCreateData> ParseModelDataCreate(string from, string to, int carId, string dateAsString, int driverId)
        {
            ParsedDriveUpdateData data = await this.ParseModelData(from, to, carId, dateAsString);
            ParsedDriveCreateData parsedData = this.mapper.Map<ParsedDriveCreateData>(data);

            parsedData.Driver = this.GetDriver(driverId);

            return parsedData;
        }        

        private async Task<ParsedDriveUpdateData> ParseModelData(string from, string to, int carId, string dateAsString)
        {
            try
            {
                City cityFrom = await this.citiesService.GetOrCreateAsync(from);
                City cityTo = await this.citiesService.GetOrCreateAsync(to);
                Car car = this.carsService.GetById(carId);
                DateTime date = DateTime.ParseExact(dateAsString, "dd/MM/yyyy - HH:mm", CultureInfo.CurrentCulture);

                var parsedData = new ParsedDriveUpdateData()
                {
                    From = cityFrom,
                    To = cityTo,
                    Car = car,
                    Date = date
                };

                return parsedData;
            }
            catch(FormatException dateException)
            {
                // Log exception
                throw new InvalidDateFormatException("Given was format invalid date.");
            }
            
        }

        private ApplicationUser GetDriver(int id)
        {
            return this.usersService.GetById(id);
        }        
    }
}
