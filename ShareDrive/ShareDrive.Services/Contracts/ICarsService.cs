﻿using ShareDrive.Models;
using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.CarViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface ICarsService
    {
        Task Create(CarCreateViewModel model, int ownerId);

        CarEditViewModel GetEditViewModel(int id);

        Task<bool> Edit(int id, CarEditViewModel model);

        CarDeleteViewModel GetDeleteViewModel(int id);

        bool Delete(int id);

        Car GetById(int id);

        IEnumerable<CarIndexViewModel> GetAllCarsIndex(int? userId);





        IEnumerable<ShareDrive.ViewModels.Admin.Car.CarAdminIndexViewModel> GetAllAdmin();

        ShareDrive.ViewModels.Admin.Car.CarAdminDetailsViewModel GetDetailsAdmin(int id);


        
        
        

        List<CarSelectViewModel> GetSelectionListByDriver(int id);

        
    }
}
