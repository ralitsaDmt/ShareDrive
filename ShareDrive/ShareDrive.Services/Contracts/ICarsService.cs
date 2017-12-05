using ShareDrive.Models;
using ShareDrive.ViewModels.CarViewModels;
using System.Linq;

namespace ShareDrive.Services.Contracts
{
    public interface ICarsService
    {
        void Create(Car car);

        IQueryable<IndexViewModel> GetAllCarsIndex();
    }
}
