using ShareDrive.ViewModels.Car;
using System;
using System.Collections.Generic;

namespace ShareDrive.ViewModels.Drive
{
    public class CreateViewModel
    {
        public string From { get; set; }
        
        public string To { get; set; }

        public string LocationToPick { get; set; }

        public string LocationToArrive { get; set; }

        public string DateTime { get; set; }

        //public DateTime Time { get; set; }

        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        public List<SelectViewModel> Cars { get; set; }

        public int CarId { get; set; }


    }
}
