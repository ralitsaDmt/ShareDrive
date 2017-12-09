using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface IDrivesService
    {
        void Create(CreateViewModel model, City cityFrom, City cityTo, int userId);
    }
}
