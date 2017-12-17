using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShareDrive.Common;
using ShareDrive.Models;
using ShareDrive.Services.Contracts;
using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.Drive;
using ShareDrive.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ShareDrive.Services.Models;

namespace ShareDrive.Services
{
    public class DrivesService : IDrivesService
    {
        private readonly IDbRepository<Drive> drives;
        private readonly IMapper mapper;

        public DrivesService(IDbRepository<Drive> drives, IMapper mapper)
        {
            this.drives = drives;
            this.mapper = mapper;
        }

        public void Update(DriveCreateEditViewModel model, ParsedDriveUpdateData data, int driveId)
        {
            Drive drive = this.drives.GetById(driveId);
            this.MapDrive(model, data, drive);
            this.drives.Update(drive);
        }

        public async Task Create(DriveCreateEditViewModel model, ParsedDriveCreateData data)
        {
            Drive drive = this.MapDriveCreate(model, data);
            await this.drives.CreateAsync(drive);
        }

        private Drive MapDriveCreate(DriveCreateEditViewModel model, ParsedDriveCreateData data)
        {
            Drive drive = this.MapDrive(model, data);
            drive.Driver = data.Driver;

            return drive;
        }
        
        private Drive MapDrive(DriveCreateEditViewModel model, BaseParsedDriveData data, Drive drive = null)
        {
            if (drive == null)
            {
                drive = new Drive();
                drive = this.mapper.Map<Drive>(model);
            }
            else
            {
                drive.DeclaredSeats = model.DeclaredSeats;
                drive.LocationToPick = model.LocationToPick;
                drive.LocationToArrive = model.LocationToArrive;
                drive.Price = model.Price;
            }            

            drive.From = data.From;
            drive.To = data.To;
            drive.Car = data.Car;
            drive.DateTime = data.Date;

            return drive;
        }

        public IEnumerable<DriveIndexViewModel> GetAll()
        {
            return this.drives.GetAll()
                .OrderByDescending(d => d.DateTime)
                .ProjectTo<DriveIndexViewModel>().ToList();
        }




        public List<ShareDrive.ViewModels.Drive.DriveIndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null)
        {
            IQueryable<Drive> drives = this.drives.GetAll();

            if (!string.IsNullOrEmpty(from))
            {
                drives = drives.Where(d => d.From.Name == from);
            }

            if (!string.IsNullOrEmpty(to))
            {
                drives = drives.Where(d => d.To.Name == to);
            }

            if (!string.IsNullOrEmpty(date))
            {
                DateTime datetime = DateTime.ParseExact(date, "dd MM yyyy", CultureInfo.InvariantCulture);
                drives = drives.Where(d => d.DateTime.Date == datetime);
            }
        
            switch (sort)
            {
                case "Date": drives = drives.OrderBy(d => d.DateTime);
                    break;
                case "date_desc": drives = drives.OrderByDescending(d => d.DateTime);
                    break;
            }

            return drives.ProjectTo<ShareDrive.ViewModels.Drive.DriveIndexViewModel>().ToList();
        }


        public IEnumerable<ShareDrive.ViewModels.Admin.Drive.DriveAdminIndexViewModel> GetAllAdmin()
        {
            var drives = this.drives.GetAll()
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.Driver)
                .Include(d => d.Car)
                .Include(d => d.DrivesPassengers)
                .ProjectTo<ShareDrive.ViewModels.Admin.Drive.DriveAdminIndexViewModel>()
                .ToList();

            return drives;
        }

        public async Task CreateAsync (DriveCreateEditViewModel model, City cityFrom, City cityTo, int userId)
        {
            //Drive drive = this.GetMappedDrive(model, cityFrom, cityTo);
            //drive.DriverId = userId;
            //await this.drives.CreateAsync(drive);
        }

        

        public ShareDrive.ViewModels.Drive.DriveCollectionsViewModel GetDetailsModel(int id, int userId)
        {
            IQueryable<Drive> driveAsQueryable = this.drives.GetByIdQueryable(id);

            Drive drive = driveAsQueryable
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.Car)
                .Include(d => d.Driver)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            DriveDetailsViewModel driveViewModel = mapper.Map<DriveDetailsViewModel>(drive);
            driveViewModel.ReservedByUser = drive.DrivesPassengers.Any(x => x.PassengerId == userId);
            driveViewModel.UserIsOwner = drive.DriverId == userId;

            DriverDetailsViewModel driverViewModel = mapper.Map<DriverDetailsViewModel>(drive.Driver);
            CarDetailsViewModel carViewModel = mapper.Map<CarDetailsViewModel>(drive.Car);

            ShareDrive.ViewModels.Drive.DriveCollectionsViewModel model = new ShareDrive.ViewModels.Drive.DriveCollectionsViewModel()
            {
                Drive = driveViewModel,
                Driver = driverViewModel,
                Car = carViewModel
            };

            return model;
        }
        

        public DriveCreateEditViewModel GetEditModelById(int id)
        {
            IQueryable<Drive> drive = this.drives.GetByIdQueryable(id);

            Drive driveModel = drive
                .Include(x => x.From)
                .Include(x => x.To)
                .FirstOrDefault();

            DriveCreateEditViewModel model = this.mapper.Map<DriveCreateEditViewModel>(driveModel);

            return model;
        }

        public DriveDeleteViewModel GetDeleteModelById(int id)
        {
            IQueryable<Drive> drive = this.drives.GetByIdQueryable(id);

            Drive driveModel = drive
                .Include(x => x.From)
                .Include(x => x.To)
                .Include(x => x.Car)
                .FirstOrDefault();

            DriveDeleteViewModel model = this.mapper.Map<DriveDeleteViewModel>(driveModel);

            return model;
        }

        public Drive GetById(int id)
        {
            return this.drives.GetById(id);
        }

        public bool Delete(int id)
        {
            Drive drive = this.drives.GetById(id);
            return this.drives.Delete(drive);
        }

        public KeyValuePair<bool, string> ReserveSeat(int driveId, int userId)
        {
            // get the drive with the passengers
            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            // Check if the user has a reservation for the drive
            if (drive.DrivesPassengers.Any(x => x.PassengerId == userId))
            {
                return new KeyValuePair<bool, string>(false, "You already have a reservation for the drive.");
            }

            // check if there are seats available
            // no => return error message that the seat is reserved by another user
            if (drive.DrivesPassengers.Count == drive.DeclaredSeats)
            {
                return new KeyValuePair<bool, string>(false, "The seat is already reserved by another user.");
            }

            // yes => add current user to the Passengers collection of the drive
            DrivesPassengers pair = new DrivesPassengers()
            {
                DriveId = driveId,
                PassengerId = userId
            };
            drive.DrivesPassengers.Add(pair);

            this.drives.Update(drive);

            return new KeyValuePair<bool, string>(true, "You made a reservation! Have a safe drive.");
        }

        public KeyValuePair<bool, string> CancelReservation(int driveId, int userId)
        {
            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            if (!drive.DrivesPassengers.Any(d => d.PassengerId == userId))
            {
                return new KeyValuePair<bool, string>(false, "You don't have a reservation for that drive.");
            }

            DrivesPassengers itemToRemove = drive.DrivesPassengers
                .FirstOrDefault(x => x.DriveId == driveId && x.PassengerId == userId);

            drive.DrivesPassengers.Remove(itemToRemove);
            this.drives.Update(drive);

            return new KeyValuePair<bool, string>(true, "Your registration has been canceled.");
        }

        

        public ViewModels.Admin.Drive.DriveAdminDetailsViewModel GetDetailsAdminModel(int id)
        {
            Drive drive = this.drives.GetByIdQueryable(id)
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.DrivesPassengers)
                .Include(d => d.Driver)
                .Include(d => d.Car)
                .FirstOrDefault();

            var model = this.mapper.Map<ShareDrive.ViewModels.Admin.Drive.DriveAdminDetailsViewModel>(drive);
            return model;
        }        
    }
}
