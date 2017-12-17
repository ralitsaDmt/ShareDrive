using ShareDrive.Services.Contracts;
using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System.Threading.Tasks;

namespace ShareDrive.Services
{
    public class DriveCitiesHelperService : IDriveCititesHelperService
    {
        private readonly IDrivesService drivesService;
        private readonly ICitiesService citiesService;

        public DriveCitiesHelperService(IDrivesService drivesService, ICitiesService citiesService)
        {
            this.drivesService = drivesService;
            this.citiesService = citiesService;
        }

        public async Task ProcessCreateDriveAsync(DriveCreateEditViewModel model, int userId)
        {
            City cityFrom = await this.citiesService.GetOrCreateAsync(model.From);
            City cityTo = await this.citiesService.GetOrCreateAsync(model.To);

            await this.drivesService.CreateAsync(model, cityFrom, cityTo, userId);
        }

        public async Task ProcessEditDriveAsync(DriveCreateEditViewModel model, int id)
        {
            City cityFrom = await this.citiesService.GetOrCreateAsync(model.From);
            City cityTo = await this.citiesService.GetOrCreateAsync(model.To);

            this.drivesService.Update(model, cityFrom, cityTo, id);
        }        
    }
}
