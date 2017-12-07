using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Models
{
    public class City : BaseModel
    {
        public string Name { get; set; }

        public ICollection<Drive> DrivesFrom { get; set; }

        public ICollection<Drive> DrivesTo { get; set; }
    }
}
