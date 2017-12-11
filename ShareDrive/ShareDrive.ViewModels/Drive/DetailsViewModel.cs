using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.User;

namespace ShareDrive.ViewModels.Drive
{
    public class DetailsViewModel
    {
        public DetailsViewModel()
        {
            this.Drive = new DriveDetailsViewModel();
            this.Driver = new DriverDetailsViewModel();
            this.Car = new CarDetailsViewModel();
        }

        public DriveDetailsViewModel Drive { get; set; }
        
        public DriverDetailsViewModel Driver { get; set; }

        public CarDetailsViewModel Car { get; set; }        
    }
}
