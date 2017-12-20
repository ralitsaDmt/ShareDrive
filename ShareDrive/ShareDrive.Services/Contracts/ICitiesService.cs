using ShareDrive.Models;
using ShareDrive.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface ICitiesService
    {
        Task<City> GetOrCreateAsync(string name);

        // List<CitySelectViewModel> GetAll();
    }
}
