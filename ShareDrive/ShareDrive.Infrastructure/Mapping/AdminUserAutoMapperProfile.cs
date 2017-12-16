using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Admin.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class AdminUserAutoMapperProfile : Profile
    {
        public AdminUserAutoMapperProfile()
        {
            CreateMap<ApplicationUser, IndexViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(source => source.FirstName + " " + source.LastName));

            CreateMap<ApplicationUser, DetailsViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(source => source.FirstName + " " + source.LastName))
                .ForMember(dest => dest.DrivesAsDriverCount,
                    opt => opt.MapFrom(source => source.Drives.Count))
                .ForMember(dest => dest.DrivesAsPassengerCount,
                    opt => opt.MapFrom(source => source.DrivesPassengers.Count))
                .ForMember(dest => dest.CarsCount, opt => opt.MapFrom(source => source.Cars.Count))
                .ForMember(dest => dest.Cars,
                    opt => opt.MapFrom(source => source.Cars.Select(c => c.ToString()).ToList()));
        }
    }
}
