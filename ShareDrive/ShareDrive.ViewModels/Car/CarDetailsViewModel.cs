using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShareDrive.ViewModels.Car
{
    public class CarDetailsViewModel
    {
        [DisplayName("Brand:")]
        public string Brand { get; set; }
        
        [DisplayName("Model:")]
        public string CarModel { get; set; }
    
        [DisplayName("Year:")]
        public int Year { get; set; }

        [DisplayName("Air Conditioner:")]
        public string HasAirConditioner { get; set; }

        // TODO: Add Number of seats total

        public byte[] Image { get; set; }
        
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
