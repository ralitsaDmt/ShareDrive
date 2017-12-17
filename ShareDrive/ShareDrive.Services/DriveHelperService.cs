using ShareDrive.Services.Contracts;
using System;
using ShareDrive.ViewModels.Drive;
using System.Threading.Tasks;
using ShareDrive.Models;
using System.Globalization;
using ShareDrive.Services.Models;
using AutoMapper;

namespace ShareDrive.Services
{
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

        public async Task ProcessCreateDriveAsync(DriveCreateEditViewModel model, int userId)
        {
            var parsedData = await this.ParseModelDataCreate(model.From, model.To, model.CarId, model.DateTime, userId);
            await this.drivesService.Create(model, parsedData);
        }

        private async Task<ParsedDriveCreateData> ParseModelDataCreate(string from, string to, int carId, string dateAsString, int driverId)
        {
            ParsedDriveUpdateData data = await this.ParseModelData(from, to, carId, dateAsString);
            ParsedDriveCreateData parsedData = this.mapper.Map<ParsedDriveCreateData>(data);

            parsedData.Driver = this.GetDriver(driverId);

            return parsedData;
        }

        public async Task ProcessEditDriveAsync(DriveCreateEditViewModel model, int driveId)
        {
            var parsedData = await this.ParseModelData(model.From, model.To, model.CarId, model.DateTime);
            this.drivesService.Update(model, parsedData, driveId);
        }

        private async Task<ParsedDriveUpdateData> ParseModelData(string from, string to, int carId, string dateAsString)
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

        private ApplicationUser GetDriver(int id)
        {
            return this.usersService.GetById(id);
        }



        public DriveCreateEditViewModel GetCreateViewModel(int userId)
        {
            throw new NotImplementedException();
        }

        public DriveCreateEditViewModel GetEditViewModel(int driveId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
