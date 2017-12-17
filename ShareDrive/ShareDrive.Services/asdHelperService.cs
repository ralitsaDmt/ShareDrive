using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System.Threading.Tasks;

namespace ShareDrive.Services
{
    public class asdHelperService : IDriveHelperService
    {
        private readonly IDrivesService drivesService;
        private readonly ICitiesService citiesService;

        public DriveHelperService(IDrivesService drivesService, ICitiesService citiesService)
        {
            this.drivesService = drivesService;
            this.citiesService = citiesService;
        }

        public async Task ProcessCreateDriveAsync(DriveEditViewModel model, int userId)
        {
            City cityFrom = await this.citiesService.GetOrCreateAsync(model.From);
            City cityTo = await this.citiesService.GetOrCreateAsync(model.To);

            await this.drivesService.CreateAsync(model, cityFrom, cityTo, userId);
        }

        public async Task ProcessEditDriveAsync(DriveEditViewModel model, int id)
        {
            City cityFrom = await this.citiesService.GetOrCreateAsync(model.From);
            City cityTo = await this.citiesService.GetOrCreateAsync(model.To);

            this.drivesService.Update(model, cityFrom, cityTo, id);
        }        
    }
}
