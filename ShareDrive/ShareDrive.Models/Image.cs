using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Models
{
    public class Image : BaseModel
    {
        public byte[] File { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }
    }
}
