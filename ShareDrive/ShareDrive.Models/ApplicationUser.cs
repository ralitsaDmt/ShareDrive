using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ShareDrive.Models
{
    public class ApplicationUser : IdentityUser<int>, IEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Car> Cars { get; set; }

        public ICollection<Drive> Drives { get; set; }

        public ICollection<DrivesPassengers> DrivesPassengers { get; set; }
    }
}
