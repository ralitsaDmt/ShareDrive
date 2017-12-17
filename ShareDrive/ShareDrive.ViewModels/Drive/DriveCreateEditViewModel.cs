using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.Drive.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareDrive.ViewModels.Drive
{
    public class DriveCreateEditViewModel : BaseEditViewModel
    {
        public List<CarSelectViewModel> Cars { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Please, select a vehicle.")]
        public int CarId { get; set; }
    }
}
