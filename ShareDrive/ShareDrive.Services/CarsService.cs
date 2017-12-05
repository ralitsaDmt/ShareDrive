using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.Common;
using ShareDrive.ViewModels.CarViewModels;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace ShareDrive.Services
{
    public class CarsService : ICarsService
    {
        private readonly IDbRepository<Car> cars;

        public CarsService(IDbRepository<Car> cars)
        {
            this.cars = cars;
        }

        public void Create(Car car)
        {
            this.cars.Create(car);
        }

        public IQueryable<IndexViewModel> GetAllCarsIndex()
        {
            return this.cars.GetAll().ProjectTo<IndexViewModel>();
        }
    }
}
