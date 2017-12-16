using ShareDrive.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.ViewModels.Admin.Car
{
    public class DetailsViewModel : CarBaseDisplayViewModel
    {
        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        public int DrivesCount { get; set; }

        public string AirConditioner { get; set; }
    }
}
