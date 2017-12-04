using System.Collections.Generic;

namespace ShareDrive.ViewModels.CarViewModels
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string CarModel { get; set; }

        public bool HasAirConditioner { get; set; }

        public List<byte[]> Images { get; set; }
    }
}
