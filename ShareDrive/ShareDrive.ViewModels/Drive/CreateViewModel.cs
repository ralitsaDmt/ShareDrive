using ShareDrive.ViewModels.Car;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareDrive.ViewModels.Drive
{
    public class CreateViewModel
    {


        [Required]
        public string From { get; set; }
        
        [Required]
        public string To { get; set; }

        [Required]
        public string LocationToPick { get; set; }

        [Required]
        public string LocationToArrive { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MM yyyy - HH:mm}")]
        public string DateTime { get; set; }
        
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be a positive number")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Value exceeds max passengers number")]
        public int AvailableSeats { get; set; }

        public List<SelectViewModel> Cars { get; set; }

        [Required]
        public int CarId { get; set; }
    }
}
