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
using System.Text;

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

        public List<IndexViewModel> GetAll(string sort, string from = null, string to = null, string date = null)
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

            return drives.ProjectTo<IndexViewModel>().ToList();
        }


        public void Create (EditViewModel model, City cityFrom, City cityTo, int userId)
        {
            Drive drive = this.GetMappedDrive(model, cityFrom, cityTo);
            drive.DriverId = userId;
            this.drives.Create(drive);
        }

        public void Update(EditViewModel model, City cityFrom, City cityTo, int id)
        {
            Drive drive = this.GetMappedDrive(model, cityFrom, cityTo);
            drive.Id = id;
            this.drives.Update(drive);
        }

        public DetailsViewModel GetDetailsModel(int id, int userId)
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

            DetailsViewModel model = new DetailsViewModel()
            {
                Drive = driveViewModel,
                Driver = driverViewModel,
                Car = carViewModel
            };

            return model;
        }

        private Drive GetMappedDrive(EditViewModel model, City cityFrom, City cityTo)
        {
            Drive drive = this.mapper.Map<Drive>(model);

            drive.DateTime = this.GetDateFromModel(model.DateTime);

            drive.From = cityFrom;
            drive.To = cityTo;

            return drive;
        }

        public EditViewModel GetEditModelById(int id)
        {
            IQueryable<Drive> drive = this.drives.GetByIdQueryable(id);

            Drive driveModel = drive
                .Include(x => x.From)
                .Include(x => x.To)
                .FirstOrDefault();

            EditViewModel model = this.mapper.Map<EditViewModel>(driveModel);

            return model;
        }

        public DeleteViewModel GetDeleteModelById(int id)
        {
            IQueryable<Drive> drive = this.drives.GetByIdQueryable(id);

            Drive driveModel = drive
                .Include(x => x.From)
                .Include(x => x.To)
                .Include(x => x.Car)
                .FirstOrDefault();

            DeleteViewModel model = this.mapper.Map<DeleteViewModel>(driveModel);

            return model;
        }

        public Drive GetById(int id)
        {
            return this.drives.GetById(id);
        }

        public void Delete(int id)
        {
            Drive drive = this.drives.GetById(id);
            this.drives.Delete(drive);
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

        private DateTime GetDateFromModel(string modelDate)
        {
            string[] tokens = modelDate.Split('-');

            int[] dateTokens = this.GetTokens(tokens[0], "/");
            int[] timeTokens = this.GetTokens(tokens[1], ":");

            return new DateTime(dateTokens[2], dateTokens[1], dateTokens[0], timeTokens[0], timeTokens[1], 0);
        }

        private int[] GetTokens(string input, string separator)
        {
            int[] tokens = input.Trim()
                .Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();

            return tokens;
        }
    }
}
