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
        }
    }
}
