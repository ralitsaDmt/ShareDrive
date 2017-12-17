using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Admin.Car;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class AdminCarAutoMapperProfile : Profile
    {
        public AdminCarAutoMapperProfile()
        {
            CreateMap<Car, CarAdminIndexViewModel>();

            CreateMap<Car, CarAdminDetailsViewModel>()
                .ForMember(dest => dest.DrivesCount, opt => opt.MapFrom(source => source.Drives.Count))
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(source => source.Owner.FirstName + " " + source.Owner.LastName))
                .ForMember(dest => dest.AirConditioner, opt => opt.MapFrom(source => source.HasAirConditioner ? "Yes" : "No"))
                .ForMember(dest => dest.ImageSource, opt => opt.Ignore());
        }
    }
}
