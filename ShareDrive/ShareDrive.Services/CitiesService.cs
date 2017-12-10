using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.Common;
using System.Linq;
using ShareDrive.ViewModels.City;
using AutoMapper.QueryableExtensions;

namespace ShareDrive.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly IDbRepository<City> cities;

        public CitiesService(IDbRepository<City> cities)
        {
            this.cities = cities;
        }
        
        public City GetOrCreate(string name)
        {
            City city = this.cities.GetAll().Where(c => c.Name == name).FirstOrDefault();

            if (city == null)
            {
                city = this.Create(name);
            }

            return city;
        }

        public List<SelectViewModel> GetAll()
        {
            return this.cities.GetAll().ProjectTo<SelectViewModel>().ToList();
        }

        private City Create(string name)
        {
            City city = new City()
            {
                Name = name
            };
            return this.cities.Create(city);
        }
    }
}
