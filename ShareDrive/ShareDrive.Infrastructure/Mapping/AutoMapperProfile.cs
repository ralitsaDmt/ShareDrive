using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.CarViewModels;
using System;

namespace ShareDrive.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Car, CreateViewModel>();

            CreateMap<Car, IndexViewModel>();
        }
    }
}
