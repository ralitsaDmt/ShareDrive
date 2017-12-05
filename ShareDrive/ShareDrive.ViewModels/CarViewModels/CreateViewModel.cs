using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShareDrive.ViewModels.CarViewModels
{
    public class CreateViewModel
    {
        [Required]
        public string Brand { get; set; }

        [Required]
        public string CarModel { get; set; }

        [Required]
        public int Year { get; set; }

        public bool HasAirConditioner { get; set; }

        public IFormFile Image { get; set; }
    }
}
