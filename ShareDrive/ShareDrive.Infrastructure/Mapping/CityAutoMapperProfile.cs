using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.City;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class CityAutoMapperProfile : Profile
    {
        public CityAutoMapperProfile()
        {
            CreateMap<City, SelectViewModel>();
        }
    }
}
