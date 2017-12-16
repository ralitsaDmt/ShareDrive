using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareDrive.Models
{
    public class Car : IEntity
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string CarModel { get; set; }

        public int Year { get; set; }

        public bool HasAirConditioner { get; set; }

        public byte[] Image { get; set; }

        public int OwnerId { get; set; }

        public ApplicationUser Owner { get; set; }

        public ICollection<Drive> Drives { get; set; }

        public override string ToString()
        {
            return this.Brand + " " + this.CarModel + " " + this.Year;
        }
    }
}
