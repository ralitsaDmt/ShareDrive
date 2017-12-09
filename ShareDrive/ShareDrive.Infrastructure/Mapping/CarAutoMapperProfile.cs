using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Car;
using ShareDrive.ViewModels.CarViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class CarAutoMapperProfile : Profile
    {
        public CarAutoMapperProfile()
        {
            CreateMap<Car, CreateViewModel>();

            CreateMap<CreateViewModel, Car>()
                .ForMember(car => car.Id, opt => opt.Ignore())
                .ForMember(car => car.Image, opt => opt.Ignore());

            CreateMap<Car, IndexViewModel>();

            CreateMap<Car, EditViewModel>()
                .ForMember(dest => dest.NewImage, opt => opt.Ignore())
                .ForMember(dest => dest.ImageSource, opt => opt.Ignore());

            CreateMap<Car, DeleteViewModel>();

            CreateMap<Car, SelectViewModel>()
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(source => source.Brand + " " + source.CarModel + " (" + source.Year + ")"));
        }        
    }
}
