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
            CreateMap<Car, CarCreateViewModel>();

            CreateMap<CarCreateViewModel, Car>()
                .ForMember(car => car.Id, opt => opt.Ignore())
                .ForMember(car => car.Image, opt => opt.Ignore());

            CreateMap<Car, CarIndexViewModel>();

            CreateMap<Car, CarEditViewModel>()
                .ForMember(dest => dest.NewImage, opt => opt.Ignore())
                .ForMember(dest => dest.ImageSource, opt => opt.Ignore());

            CreateMap<Car, CarDeleteViewModel>();

            CreateMap<Car, CarSelectViewModel>()
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(source => source.Brand + " " + source.CarModel + " (" + source.Year + ")"));

            CreateMap<Car, CarDetailsViewModel>()
                .ForMember(dest => dest.HasAirConditioner, opt => opt.MapFrom(source => source.HasAirConditioner ? "Yes" : "No"))
                .ForMember(dest => dest.ImageSource, opt => opt.Ignore());
        }        
    }
}
