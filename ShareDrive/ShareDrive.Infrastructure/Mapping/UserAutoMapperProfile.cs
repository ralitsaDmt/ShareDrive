using AutoMapper;
using ShareDrive.Models;
using ShareDrive.Models.AccountViewModels;
using ShareDrive.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(source => source.Email));

            CreateMap<ApplicationUser, DriverDetailsViewModel>()
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(source => source.FirstName + " " + source.LastName));
        }
    }
}
