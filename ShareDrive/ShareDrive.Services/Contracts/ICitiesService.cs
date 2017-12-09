using ShareDrive.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Contracts
{
    public interface ICitiesService
    {
        City GetOrCreate(string name);
    }
}
