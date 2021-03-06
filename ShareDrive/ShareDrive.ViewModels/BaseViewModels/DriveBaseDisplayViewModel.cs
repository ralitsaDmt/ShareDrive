﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.ViewModels.BaseViewModels
{
    public abstract class DriveBaseDisplayViewModel
    {
        public int Id { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Price { get; set; }

        public int AvailableSeats { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int DriverId { get; set; }
    }
}
