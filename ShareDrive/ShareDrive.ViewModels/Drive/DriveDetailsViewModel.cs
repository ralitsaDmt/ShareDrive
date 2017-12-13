using ShareDrive.ViewModels.Drive.BaseModels;

namespace ShareDrive.ViewModels.Drive
{
    public class DriveDetailsViewModel : BaseDisplayViewModel
    {
        public string LocationToPick { get; set; }

        public string LocationToArrive { get; set; }

        public bool ReservedByUser { get; set; }

        public bool UserIsOwner { get; set; }
    }
}
