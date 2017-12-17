using ShareDrive.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.ViewModels.Admin.Drive
{
    public class DriveAdminDetailsViewModel : DriveBaseDisplayViewModel
    {
        public int DeclaredSeats { get; set; }

        public int PassengersCount { get; set; }

        public List<string> PassengersNames { get; set; }

        public string LocationToPick { get; set; }

        public string LocationToArrive { get; set; }

        public string DriverName { get; set; }

        public int CarId { get; set; }

        public string CarIdentifier { get; set; }
    }
}
