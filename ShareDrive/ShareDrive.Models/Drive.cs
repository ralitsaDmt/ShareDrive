using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Models
{
    public class Drive : BaseModel
    {
        public int? FromId { get; set; }

        public City From { get; set; }

        public int? ToId { get; set; }

        public City To { get; set; }

        public string LocationToPick { get; set; }

        public string LocationToArrive { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        public int? CarId { get; set; }

        public Car Car { get; set; }

        public int? DriverId { get; set; }

        public ApplicationUser Driver { get; set; }
    }
}
