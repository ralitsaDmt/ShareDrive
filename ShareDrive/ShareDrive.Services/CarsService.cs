﻿using ShareDrive.Services.Contracts;
using ShareDrive.Models;
using ShareDrive.Common;
using ShareDrive.ViewModels.CarViewModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;
using ShareDrive.ViewModels.Car;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShareDrive.Services
{
    public class CarsService : ICarsService
    {
        private readonly IDbRepository<Car> cars;
        private readonly IMapper mapper;

        public CarsService(IDbRepository<Car> cars, IMapper mapper)
        {
            this.cars = cars;
            this.mapper = mapper;
        }

        public IEnumerable<ShareDrive.ViewModels.Admin.Car.IndexViewModel> GetAllAdmin()
        {
            return this.cars.GetAll()
                .Include(c => c.Drives)
                .ProjectTo<ShareDrive.ViewModels.Admin.Car.IndexViewModel>()
                .ToList();
        }

        public async Task<bool> Create(CreateViewModel model, int ownerId)
        {
            var tempPath = Path.GetTempPath();

            Car car = this.mapper.Map<Car>(model);

            using (var ms = new MemoryStream())
            {
                await model.Image.CopyToAsync(ms);
                car.Image = ms.ToArray();
            }

            car.OwnerId = ownerId;

            try
            {
                this.cars.Create(car);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }            
        }

        public EditViewModel GetEditViewModel(int id)
        {
            Car car = this.cars.GetById(id);
            EditViewModel model = this.mapper.Map<EditViewModel>(car);

            return model;
        }

        public IQueryable<IndexViewModel> GetAllCarsIndex(int? userId)
        {
            IQueryable<Car> cars = this.cars.GetAll();

            if (userId != null)
            {
                cars = cars.Where(c => c.OwnerId == userId);
            }

            return cars.ProjectTo<IndexViewModel>();
        }

        public IQueryable<Car> GetById(int id)
        {
            return this.cars.GetAll().Where(x => x.Id == id);
        }
        
        public async Task<bool> Edit(int id, EditViewModel model)
        {
            Car car = this.cars.GetById(id);
            
            if (car != null)
            {
                car.Brand = model.Brand;
                car.CarModel = model.CarModel;
                car.Year = model.Year;
                car.HasAirConditioner = model.HasAirConditioner;

                if (model.NewImage != null)
                {
                    var tempPath = Path.GetTempPath();

                    using (var ms = new MemoryStream())
                    {
                        await model.NewImage.CopyToAsync(ms);
                        car.Image = ms.ToArray();
                    }
                }

                bool result = this.cars.Update(car);

                return result;
            }

            return false;
        }

        public bool Delete(int id)
        {
            Car car = this.cars.GetById(id);

            if (car != null)
            {
                bool result = this.cars.Delete(car);
                return result;
            }
            else
            {
                return false;
            }
        }

        public List<SelectViewModel> GetSelectionListByDriver(int id)
        {
            var carsList = this.cars.GetAll()
                .Where(x => x.OwnerId == id)
                .ProjectTo<SelectViewModel>().ToList();
            return carsList;
        }

        public ShareDrive.ViewModels.Admin.Car.DetailsViewModel GetDetailsAdmin(int id)
        {
            var car = this.cars.GetByIdQueryable(id)
                .Include(c => c.Drives)
                .Include(c => c.Owner)
                .FirstOrDefault();
            var model = this.mapper.Map<ViewModels.Admin.Car.DetailsViewModel>(car);
            return model;
        }
    }
}
