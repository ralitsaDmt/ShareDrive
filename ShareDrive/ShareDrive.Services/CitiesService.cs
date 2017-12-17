using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.Common;
using System.Linq;
using ShareDrive.ViewModels.City;
using AutoMapper.QueryableExtensions;
using System.Threading.Tasks;

namespace ShareDrive.Services
{
    public class CitiesService : ICitiesService
    {
        private readonly IDbRepository<City> cities;

        public CitiesService(IDbRepository<City> cities)
        {
            this.cities = cities;
        }
        
        public async Task<City> GetOrCreateAsync(string name)
        {
            City city = this.cities.GetAll().Where(c => c.Name == name).FirstOrDefault();

            if (city == null)
            {
                city = await this.CreateAsync(name);
            }

            return city;
        }

        public List<CitySelectViewModel> GetAll()
        {
            return this.cities.GetAll().ProjectTo<CitySelectViewModel>().ToList();
        }

        private async Task<City> CreateAsync(string name)
        {
            City city = new City()
            {
                Name = name
            };
            return await this.cities.CreateAsync(city);
        }
    }
}
