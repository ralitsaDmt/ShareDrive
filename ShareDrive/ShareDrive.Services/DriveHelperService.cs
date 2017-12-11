using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;

namespace ShareDrive.Services
{
    public class DriveHelperService : IDriveHelperService
    {
        private readonly IDrivesService drivesService;
        private readonly ICitiesService citiesService;

        public DriveHelperService(IDrivesService drivesService, ICitiesService citiesService)
        {
            this.drivesService = drivesService;
            this.citiesService = citiesService;
        }

        public void ProcessCreateDrive(EditViewModel model, int userId)
        {
            City cityFrom = this.citiesService.GetOrCreate(model.From);
            City cityTo = this.citiesService.GetOrCreate(model.To);

            this.drivesService.Create(model, cityFrom, cityTo, userId);
        }

        public void ProcessEditDrive(EditViewModel model, int id)
        {
            City cityFrom = this.citiesService.GetOrCreate(model.From);
            City cityTo = this.citiesService.GetOrCreate(model.To);

            this.drivesService.Update(model, cityFrom, cityTo, id);
        }        
    }
}
