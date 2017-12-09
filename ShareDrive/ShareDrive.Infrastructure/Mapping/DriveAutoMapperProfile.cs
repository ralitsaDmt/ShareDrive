using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class DriveAutoMapperProfile : Profile
    {
        public DriveAutoMapperProfile()
        {
            CreateMap<CreateViewModel, Drive>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Car, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.Ignore());
        }
    }
}
