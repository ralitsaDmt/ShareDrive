using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Drive;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class DriveAutoMapperProfile : Profile
    {
        public DriveAutoMapperProfile()
        {
            CreateMap<EditViewModel, Drive>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Car, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.Ignore());

            CreateMap<Drive, EditViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.Cars, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime.ToString("o")));

            CreateMap<Drive, DeleteViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime.ToString("o")))
                .ForMember(dest => dest.Car, opt => opt.MapFrom(source => source.Car.Brand + " " + source.Car.CarModel + " (" + source.Car.Year + ")"));

            CreateMap<Drive, IndexViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats, 
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(source => source.DateTime.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")));

            CreateMap<Drive, DetailsViewModel>();

            CreateMap<Drive, DriveDetailsViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats, 
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(source => source.DateTime.ToString("dd MM yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")));
        }
    }
}
