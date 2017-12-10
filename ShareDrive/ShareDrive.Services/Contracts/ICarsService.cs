using ShareDrive.Models;
using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.CarViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Services.Contracts
{
    public interface ICarsService
    {
        Task<bool> Create(CreateViewModel model, int ownerId);

        EditViewModel GetEditViewModel(int id);

        Task<bool> Edit(int id, EditViewModel model);

        IQueryable<IndexViewModel> GetAllCarsIndex();

        IQueryable<Car> GetById(int id);
        
        bool Delete(int id);

        List<SelectViewModel> GetSelectionListByDriver(int id);
    }
}
