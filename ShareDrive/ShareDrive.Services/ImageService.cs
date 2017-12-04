using ShareDrive.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using ShareDrive.Models;
using ShareDrive.Common;

namespace ShareDrive.Services
{
    public class ImageService : IImageService
    {
        private readonly IDbRepository<Image> images;

        public ImageService(IDbRepository<Image> images)
        {
            this.images = images;
        }
    }
}
