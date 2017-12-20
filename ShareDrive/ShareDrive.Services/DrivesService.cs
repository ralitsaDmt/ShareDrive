namespace ShareDrive.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ShareDrive.Services.Models;
    using ShareDrive.Core;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Common;
    using ShareDrive.Models;
    using Services.Contracts;
    using ViewModels.Car;
    using ViewModels.Drive;
    using ViewModels.User;
    using ShareDrive.ViewModels.Admin.Drive;

    public class DrivesService : IDrivesService
    {
        private readonly IDbRepository<Drive> drives;
        private readonly IMapper mapper;
        private readonly IUsersService usersService;

        public DrivesService(IDbRepository<Drive> drives, IMapper mapper, IUsersService usersService)
        {
            this.drives = drives;
            this.mapper = mapper;
            this.usersService = usersService;
        }

        public IEnumerable<DriveIndexViewModel> GetAll()
        {
            var result = this.drives.GetAll()
                .OrderByDescending(d => d.DateTime)
                .ToList();

            return this.mapper.Map<List<DriveIndexViewModel>>(result);
        }

        public IEnumerable<DriveAdminIndexViewModel> GetAllAdmin()
        {
            var drives = this.drives.GetAll()
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.Driver)
                .Include(d => d.Car)
                .Include(d => d.DrivesPassengers)
                .ToList();

            return this.mapper.Map<List<DriveAdminIndexViewModel>>(drives);
        }

        public Drive GetById(int id)
        {
            Require.ThatIntIsPositive(id);

            return this.drives.GetById(id);
        }

        public async Task CreateAsync(DriveCreateEditViewModel model, ParsedDriveCreateData data)
        {
            Require.ThatObjectIsNotNull(model);
            Require.ThatObjectIsNotNull(data);

            Drive drive = this.MapDriveCreate(model, data);
            await this.drives.CreateAsync(drive);
        }

        public DriveCreateEditViewModel GetEditModelById(int id)
        {
            Require.ThatIntIsPositive(id);
            
            Drive drive = this.drives.GetByIdQueryable(id)
                .Include(x => x.From)
                .Include(x => x.To)
                .FirstOrDefault();

            if (drive != null)
            {
                return this.mapper.Map<DriveCreateEditViewModel>(drive);
            }

            return null;
        }

        public async Task<bool> UpdateAsync(DriveCreateEditViewModel model, ParsedDriveUpdateData data, int driveId)
        {
            Require.ThatObjectIsNotNull(model);
            Require.ThatObjectIsNotNull(data);
            Require.ThatIntIsPositive(driveId);
            
            Drive drive = this.drives.GetById(driveId);
            
            if (drive != null)
            {
                if (data.From != null
                    && data.To != null
                    && data.Car != null)
                {
                    this.MapDrive(model, data, drive);
                    await this.drives.UpdateAsync(drive);
                    return true;
                }

                return false;
            }

            return false;
        }

        public DriveDeleteViewModel GetDeleteModelById(int id)
        {
            Require.ThatIntIsPositive(id);

            Drive drive = this.drives.GetByIdQueryable(id)
                .Include(x => x.From)
                .Include(x => x.To)
                .Include(x => x.Car)
                .FirstOrDefault();

            if (drive != null)
            {
                return this.mapper.Map<DriveDeleteViewModel>(drive);
            }

            return null;
        }

        public bool Delete(int id)
        {
            Require.ThatIntIsPositive(id);

            Drive drive = this.drives.GetById(id);

            if (drive != null)
            {
                return this.drives.Delete(drive);
            }

            return false;
        }

        public DriveCollectionsViewModel GetDetailsModel(int driveId, int userId)
        {
            Require.ThatIntIsPositive(driveId);
            Require.ThatIntIsPositive(userId);

            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.Car)
                .Include(d => d.Driver)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            if (drive != null)
            {
                DriveDetailsViewModel driveViewModel = mapper.Map<DriveDetailsViewModel>(drive);
                driveViewModel.ReservedByUser = drive.DrivesPassengers.Any(x => x.PassengerId == userId);
                driveViewModel.UserIsOwner = drive.DriverId == userId;

                DriverDetailsViewModel driverViewModel = mapper.Map<DriverDetailsViewModel>(drive.Driver);

                CarDetailsViewModel carViewModel = mapper.Map<CarDetailsViewModel>(drive.Car);

                DriveCollectionsViewModel model = new DriveCollectionsViewModel()
                {
                    Drive = driveViewModel,
                    Driver = driverViewModel,
                    Car = carViewModel
                };

                return model;
            }

            return null;
        }

        public DriveAdminDetailsViewModel GetDetailsAdminModel(int driveId)
        {
            Require.ThatIntIsPositive(driveId);

            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.From)
                .Include(d => d.To)
                .Include(d => d.DrivesPassengers)
                .Include(d => d.Driver)
                .Include(d => d.Car)
                .FirstOrDefault();

            if (drive != null)
            {
                return this.mapper.Map<DriveAdminDetailsViewModel>(drive);
            }

            return null;
        }

        public KeyValuePair<bool, string> ReserveSeat(int driveId, int userId)
        {
            Require.ThatIntIsPositive(driveId);
            Require.ThatIntIsPositive(userId);
            
            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            if (drive == null)
            {
                return new KeyValuePair<bool, string>(false, "This drive does not exist.");
            }

            if (!this.usersService.CheckIfUserExists(userId))
            {
                return new KeyValuePair<bool, string>(false, "This user does not exist.");
            }
                        
            if (drive.DrivesPassengers.Any(x => x.PassengerId == userId))
            {
                return new KeyValuePair<bool, string>(false, "You already have a reservation for the drive.");
            }
            
            if (drive.DrivesPassengers.Count == drive.DeclaredSeats)
            {
                return new KeyValuePair<bool, string>(false, "All seats for the drive are taken.");
            }
            
            DrivesPassengers pair = new DrivesPassengers()
            {
                DriveId = driveId,
                PassengerId = userId
            };
            drive.DrivesPassengers.Add(pair);

            this.drives.UpdateAsync(drive);

            return new KeyValuePair<bool, string>(true, "You made a reservation! Have a safe drive.");
        }

        public KeyValuePair<bool, string> CancelReservation(int driveId, int userId)
        {
            Require.ThatIntIsPositive(driveId);
            Require.ThatIntIsPositive(userId);

            Drive drive = this.drives.GetByIdQueryable(driveId)
                .Include(d => d.DrivesPassengers)
                .FirstOrDefault();

            if (drive == null)
            {
                return new KeyValuePair<bool, string>(false, "This drive does not exist.");
            }

            if (!this.usersService.CheckIfUserExists(userId))
            {
                return new KeyValuePair<bool, string>(false, "This user does not exist.");
            }

            if (!drive.DrivesPassengers.Any(d => d.PassengerId == userId))
            {
                return new KeyValuePair<bool, string>(false, "You don't have a reservation for that drive.");
            }

            DrivesPassengers itemToRemove = drive.DrivesPassengers
                .FirstOrDefault(x => x.DriveId == driveId && x.PassengerId == userId);

            drive.DrivesPassengers.Remove(itemToRemove);
            this.drives.UpdateAsync(drive);

            return new KeyValuePair<bool, string>(true, "Your registration has been canceled.");
        }

        private Drive MapDriveCreate(DriveCreateEditViewModel model, ParsedDriveCreateData data)
        {
            Drive drive = this.MapDrive(model, data);

            if (drive != null && data.Driver != null)
            {
                drive.Driver = data.Driver;
                return drive;
            }

            return null;
        }
        
        private Drive MapDrive(DriveCreateEditViewModel model, BaseParsedDriveData data, Drive drive = null)
        {
            if (data.From != null && data.To != null && data.Car != null && data.Date != null)
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
            }           

            return drive;
        }

        //public List<ShareDrive.ViewModels.Drive.DriveIndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null)
        //{
        //    IQueryable<Drive> drives = this.drives.GetAll();

        //    if (!string.IsNullOrEmpty(from))
        //    {
        //        drives = drives.Where(d => d.From.Name == from);
        //    }

        //    if (!string.IsNullOrEmpty(to))
        //    {
        //        drives = drives.Where(d => d.To.Name == to);
        //    }

        //    if (!string.IsNullOrEmpty(date))
        //    {
        //        DateTime datetime = DateTime.ParseExact(date, "dd MM yyyy", CultureInfo.InvariantCulture);
        //        drives = drives.Where(d => d.DateTime.Date == datetime);
        //    }
        
        //    switch (sort)
        //    {
        //        case "Date": drives = drives.OrderBy(d => d.DateTime);
        //            break;
        //        case "date_desc": drives = drives.OrderByDescending(d => d.DateTime);
        //            break;
        //    }

        //    return drives.ProjectTo<ShareDrive.ViewModels.Drive.DriveIndexViewModel>().ToList();
        //}    
    }
}
