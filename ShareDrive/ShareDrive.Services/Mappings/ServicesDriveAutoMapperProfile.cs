using AutoMapper;
using ShareDrive.Models;
using ShareDrive.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Services.Mappings
{
    public class ServicesDriveAutoMapperProfile : Profile
    {
        public ServicesDriveAutoMapperProfile()
        {
            CreateMap<BaseParsedDriveData, ParsedDriveCreateData>();
        }
    }
}
