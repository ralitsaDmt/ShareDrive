using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Models
{
    public class DrivesPassengers
    {
        public int DriveId { get; set; }

        public Drive Drive { get; set; }

        public int PassengerId { get; set; }

        public ApplicationUser Passenger { get; set; }
    }
}
