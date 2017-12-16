using ShareDrive.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShareDrive.ViewModels.Admin.User
{
    public class DetailsViewModel : UserBaseDisplayViewModel
    {
        [DisplayName("Drives As Driver")]
        public int DrivesAsDriverCount { get; set; }

        [DisplayName("Drives As Passenger")]
        public int DrivesAsPassengerCount { get; set; }

        [DisplayName("Registered Cars")]
        public int CarsCount { get; set; }

        public List<string> Cars { get; set; }
    }
}
