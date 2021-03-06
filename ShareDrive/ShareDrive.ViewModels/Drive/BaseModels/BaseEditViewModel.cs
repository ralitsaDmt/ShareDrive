﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShareDrive.ViewModels.Drive.BaseModels
{
    public abstract class BaseEditViewModel
    {
        public int Id { get; set; }

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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy - HH:mm}")]
        public string DateTime { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Value should be a positive number.")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10.")]
        public int DeclaredSeats { get; set; }
    }
}
