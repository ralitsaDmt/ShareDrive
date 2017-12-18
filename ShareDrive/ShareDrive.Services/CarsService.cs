using ShareDrive.Services.Contracts;
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
using ShareDrive.Core;
using ShareDrive.Exceptions.Car;
using ShareDrive.ViewModels.Admin.Car;

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

        public async Task Create(CarCreateViewModel model, int ownerId)
        {
            Require.ThatObjectIsNotNull(model);
            Require.ThatIntIsPositive(ownerId);

            try
            {
                Car car = this.mapper.Map<Car>(model);

                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    car.Image = ms.ToArray();
                }

                car.OwnerId = ownerId;

                await this.cars.CreateAsync(car);
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                throw new CarCreateException("Failed has car creating.");
            }
        }

        public CarEditViewModel GetEditViewModel(int id)
        {
            Require.ThatIntIsPositive(id);

            Car car = this.cars.GetById(id);

            if (car != null)
            {
                return this.mapper.Map<CarEditViewModel>(car);
            }

            return null;
        }

        public async Task<bool> Edit(int id, CarEditViewModel model)
        {
            Require.ThatIntIsPositive(id);
            Require.ThatObjectIsNotNull(model);

            Car car = this.cars.GetById(id);

            if (car != null)
            {
                car.Brand = model.Brand;
                car.CarModel = model.CarModel;
                car.Year = model.Year;
                car.HasAirConditioner = model.HasAirConditioner;

                if (model.NewImage != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.NewImage.CopyToAsync(ms);
                        car.Image = ms.ToArray();
                    }
                }

                return this.cars.Update(car);
            }

            return false;
        }

        public CarDeleteViewModel GetDeleteViewModel(int id)
        {
            Require.ThatIntIsPositive(id);

            Car car = this.cars.GetById(id);

            if (car != null)
            {
                return this.mapper.Map<CarDeleteViewModel>(car);
            }

            return null;
        }

        public bool Delete(int id)
        {
            Require.ThatIntIsPositive(id);

            Car car = this.cars.GetById(id);

            if (car != null)
            {
                return this.cars.Delete(car);
            }

            return false;
        }

        public List<CarSelectViewModel> GetSelectionListByDriver(int id)
        {
            Require.ThatIntIsPositive(id);

            var cars = this.cars.GetAll()
                .Where(x => x.OwnerId == id)
                .ToList();

            List<CarSelectViewModel> selectCars = mapper.Map<List<CarSelectViewModel>>(cars);

            return selectCars;
        }

        public Car GetById(int id)
        {
            Require.ThatIntIsPositive(id);
            return this.cars.GetById(id);
        }

        public IEnumerable<CarIndexViewModel> GetAllCarsIndex(int? userId)
        {
            if (userId != null)
            {
                Require.ThatIntIsPositive((int)userId);

                List<Car> cars = this.cars.GetAll()
                    .Where(c => c.OwnerId == userId)
                    .ToList();

                List<CarIndexViewModel> model = this.mapper.Map<List<CarIndexViewModel>>(cars);

                return model;
            }

            return null;
        }
        
        public IEnumerable<CarAdminIndexViewModel> GetAllAdmin()
        {
            List<Car> cars = this.cars.GetAll()
                .Include(c => c.Drives)
                .ToList();

            List<CarAdminIndexViewModel> listCars = this.mapper
                .Map<List<CarAdminIndexViewModel>>(cars);

            return listCars;
        }        

        public CarAdminDetailsViewModel GetDetailsAdmin(int id)
        {
            Require.ThatIntIsPositive(id);

            var car = this.cars.GetByIdQueryable(id)
                .Include(c => c.Drives)
                .Include(c => c.Owner)
                .FirstOrDefault();

            if (car != null)
            {
                return this.mapper.Map<CarAdminDetailsViewModel>(car);
            }

            return null;
        }        
    }
}
