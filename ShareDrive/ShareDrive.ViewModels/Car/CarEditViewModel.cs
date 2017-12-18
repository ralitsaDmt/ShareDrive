

namespace ShareDrive.ViewModels.CarViewModels
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CarEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string CarModel { get; set; }

        [Required]
        public int Year { get; set; }

        public bool HasAirConditioner { get; set; }
        
        public byte[] Image { get; set; }

        public IFormFile NewImage { get; set; }

        public string ImageSource
        {
            get
            {
                if (this.Image != null)
                {
                    string mimeType = "image/png";
                    string base64 = Convert.ToBase64String(this.Image);
                    return string.Format("data:{0};base64,{1}", mimeType, base64);
                }

                return null;
            }
        }
    }
}
