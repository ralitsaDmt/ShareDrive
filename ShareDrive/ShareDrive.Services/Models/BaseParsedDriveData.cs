using ShareDrive.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Models
{
    public class BaseParsedDriveData
    {
        public City From { get; set; }

        public City To { get; set; }

        public Car Car { get; set; }

        public DateTime Date { get; set; }
    }
}
