using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.Admin.Drive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareDrive.Infrastructure.Mapping
{
    public class AdminDriveAutoMapperProfile : Profile
    {
        public AdminDriveAutoMapperProfile()
        {
            CreateMap<Drive, IndexViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(source => source.DateTime.ToString("dd/MM/yyyy HH:mm")));

            CreateMap<Drive, DetailsViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(source => source.DateTime.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")))
                .ForMember(dest => dest.PassengersCount,
                    opt => opt.MapFrom(source => source.DrivesPassengers.Count))
                .ForMember(dest => dest.PassengersNames,
                    opt => opt.MapFrom(source => source.DrivesPassengers
                                            .Select(d => d.Passenger.FirstName + " " + d.Passenger.LastName)
                                            .ToList()))
                .ForMember(dest => dest.DriverName,
                    opt => opt.MapFrom(source => source.Driver.FirstName + " " + source.Driver.LastName))
                .ForMember(dest => dest.CarIdentifier,
                    opt => opt.MapFrom(source => source.Car.Brand + " " + source.Car.CarModel + " " + source.Car.Year));
        }
    }
}
