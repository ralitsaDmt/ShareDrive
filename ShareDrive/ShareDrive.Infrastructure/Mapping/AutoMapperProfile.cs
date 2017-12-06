using AutoMapper;
using ShareDrive.Models;
using ShareDrive.ViewModels.CarViewModels;

namespace ShareDrive.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
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
        }
    }
}
