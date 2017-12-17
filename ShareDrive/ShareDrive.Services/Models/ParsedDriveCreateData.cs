using ShareDrive.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Models
{
    public class ParsedDriveCreateData : BaseParsedDriveData
    {
        public ApplicationUser Driver { get; set; }
    }
}
