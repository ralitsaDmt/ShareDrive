using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.ViewModels.BaseViewModels
{
    public abstract class CarBaseDisplayViewModel
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public int Year { get; set; }

        public string CarModel { get; set; }

        public bool HasAirConditioner { get; set; }

        public byte[] Image { get; set; }

        public string ImageSource
        {
            get
            {
                string mimeType = "image/png";
                string base64 = Convert.ToBase64String(this.Image);
                return string.Format("data:{0};base64,{1}", mimeType, base64);
            }
        }
    }
}
