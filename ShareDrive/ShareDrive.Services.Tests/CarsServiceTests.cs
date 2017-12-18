namespace ShareDrive.Services.Tests
{
    using System;
    using System.Threading.Tasks;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Http;
    using Xunit;
    using Moq;
    using FluentAssertions;
    using AutoMapper;

    using ViewModels.CarViewModels;
    using ShareDrive.Models;
    using ShareDrive.Common;
    using ShareDrive.Exceptions;
    using ShareDrive.Exceptions.Car;
    using ShareDrive.ViewModels.Car;
    using ShareDrive.ViewModels.Admin.Car;

    public class CarsServiceTests
    {
        // Create
        [Fact]
        public async Task Create_GivenValidModelAndOwnerId_ShouldReturnCompletedTask()
        {
            // arrange
            int ownerId = 1;
            CarCreateViewModel model = new CarCreateViewModel()
            {
                Brand = "TestBrand",
                CarModel = "TestModel",
                Year = 1234,
                HasAirConditioner = true,
                Image = new Mock<IFormFile>().Object
            };

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<CarCreateViewModel, Car>()
                    .ForMember(car => car.Id, opt => opt.Ignore())
                    .ForMember(car => car.Image, opt => opt.Ignore())));

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Car>()))
                .Returns(Task.FromResult(new Car()));

            var service = new CarsService(mockRepository.Object, mapper);

            // act 
            Func<Task> action = async () => await service.Create(model, ownerId);

            // assert
            action.Should().NotThrow();
        }

        [Fact]
        public async Task Create_GivenNullModel_ShouldThrowNullObjectException()
        {
            // arange
            int ownerId = 1;

            var service = new CarsService(null, null);

            // act
            Func<Task> action = () => service.Create(null, ownerId);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public async Task Create_GivenNegativeOwnerId_ShouldThrowNegativeIntException()
        {
            // arrange
            int ownerId = -1;
            CarCreateViewModel model = new CarCreateViewModel();

            CarsService service = new CarsService(null, null);

            // act
            Func<Task> action = () => service.Create(model, ownerId);

            // asert
            action.Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task Create_GivenModelWithNullImage_ShouldThrowException()
        {
            // arrange
            int ownerId = 1;
            CarCreateViewModel model = new CarCreateViewModel();

            CarsService service = new CarsService(null, null);

            // act
            Func<Task> action = () => service.Create(model, ownerId);

            // assert
            action.Should().Throw<Exception>();
        }

        [Fact]
        public async Task Create_GivenInvalidData_ShouldThrowException()
        {
            // arrange
            int ownerId = 1;
            CarCreateViewModel model = new CarCreateViewModel()
            {
                Brand = "TestBrand",
                CarModel = "TestModel",
                Year = 1234,
                HasAirConditioner = true,
                Image = new Mock<IFormFile>().Object
            };

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<CarCreateViewModel, Car>()
                    .ForMember(car => car.Id, opt => opt.Ignore())
                    .ForMember(car => car.Image, opt => opt.Ignore())));

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.CreateAsync(It.IsAny<Car>()))
                .ThrowsAsync(new InvalidOperationException());

            var service = new CarsService(mockRepository.Object, mapper);

            // act
            Func<Task> action = () => service.Create(model, ownerId);

            // assert
            action.Should().ThrowExactly<CarCreateException>()
                .WithMessage("Failed has car creating.");
        }

        // GetEditViewModel
        [Fact]
        public async Task GetEditViewModel_GivenNgativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;

            CarsService service = new CarsService(null, null);

            // act            
            // assert
            service
                .Invoking(x => x.GetEditViewModel(carId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetEditViewModel_GivenValidCarId_ShouldReturnCarEditViewModel()
        {
            int carId = 1;

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Car, CarEditViewModel>()
                    .ForMember(dest => dest.NewImage, opt => opt.Ignore())
                    .ForMember(dest => dest.ImageSource, opt => opt.Ignore())));

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.GetById(1))
                .Returns(new Car()
                {
                    Id = 1,
                    Brand = "TestBrand",
                    CarModel = "TestModel",
                    HasAirConditioner = true,
                    Year = 1234,
                    OwnerId = 1,
                    Image = new byte[1024]
                });

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            CarEditViewModel result = service.GetEditViewModel(carId);

            // assert
            result.Should().BeEquivalentTo(new CarEditViewModel()
            {
                Id = 1,
                Brand = "TestBrand",
                CarModel = "TestModel",
                HasAirConditioner = true,
                Year = 1234,
                Image = new byte[1024]
            });
        }

        [Fact]
        public async Task GetEditViewModel_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int carId = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            CarEditViewModel result = service.GetEditViewModel(carId);

            // assert
            result.Should().BeEquivalentTo<CarEditViewModel>(null);
        }

        // Edit
        [Fact]
        public async Task Edit_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;
            CarsService service = new CarsService(null, null);

            // act
            Func<Task> action = () => service.Edit(carId, null);

            // assert
            action.Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task Edit_GivenNullViewModel_ShouldThrowNullObjectException()
        {
            // arrange
            int carId = 1;
            CarsService service = new CarsService(null, null);

            // act
            Func<Task> action = () => service.Edit(carId, null);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public async Task Edit_GivenNotExistingId_ShouldReturnFalse()
        {
            // arrange
            int carId = 1;
            CarEditViewModel model = new CarEditViewModel();

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            bool result = await service.Edit(carId, model);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Edit_GivenValidModelAndId_ShouldRetrnTrue()
        {
            // arrange
            int carId = 1;

            CarEditViewModel model = new CarEditViewModel()
            {
                Brand = "BrandUpdate",
                CarModel = "ModelUpdate",
                Year = 1234,
                HasAirConditioner = false,
            };

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns(new Car()
                {
                    Brand = "Brand",
                    CarModel = "Model",
                    Year = 12,
                    HasAirConditioner = true,
                    Image = new byte[1024]
                });

            mockRepository
                .Setup(x => x.Update(It.Is<Car>(car =>
                    car.Brand == "BrandUpdate"
                    && car.CarModel == "ModelUpdate"
                    && car.Year == 1234 
                    && !car.HasAirConditioner)))
                .Returns(true);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            bool result = await service.Edit(carId, model);

            // assert
            result.Should().BeTrue();
        }

        // GetDeleteViewModel
        [Fact]
        public async Task GetDeleteViewModel_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service
                .Invoking(x => x.GetDeleteViewModel(carId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetDeleteViewModel_GivenValidCarId_ShouldReturnCarDeleteViewModel()
        {
            int carId = 1;

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Car, CarDeleteViewModel>()));

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.GetById(1))
                .Returns(new Car()
                {
                    Id = 1,
                    Brand = "TestBrand",
                    CarModel = "TestModel",
                    HasAirConditioner = true,
                    Year = 1234,
                    OwnerId = 1,
                    Image = new byte[1024]
                });

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            CarDeleteViewModel result = service.GetDeleteViewModel(carId);

            // assert
            result.Should().BeEquivalentTo(new CarDeleteViewModel()
            {
                Id = 1,
                Brand = "TestBrand",
                CarModel = "TestModel"
            });
        }

        [Fact]
        public async Task GetDeleteViewModel_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int carId = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository
                .Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            CarDeleteViewModel result = service.GetDeleteViewModel(carId);

            // assert
            result.Should().BeEquivalentTo<CarDeleteViewModel>(null);
        }

        // Delete
        [Fact]
        public async Task Delete_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service
                .Invoking(x => x.Delete(carId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task Delete_GivenNotExistingId_ShouldReturnFalse()
        {
            // arrange
            int carId = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            bool result = service.Delete(carId);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Delete_GivenValidId_ShouldRetrnTrue()
        {
            // arrange
            int carId = 1;
            
            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository.Setup(x => x.GetById(carId))
                .Returns(new Car()
                {
                    Id = 1,
                    Brand = "Brand",
                    CarModel = "Model",
                    Year = 1234,
                    HasAirConditioner = true
                });

            mockRepository
                .Setup(x => x.Delete(It.Is<Car>(car =>
                    car.Id == 1
                    && car.Brand == "Brand"
                    && car.CarModel == "Model"
                    && car.Year == 1234
                    && car.HasAirConditioner)))
                .Returns(true);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            bool result = service.Delete(carId);

            // assert
            result.Should().BeTrue();
        }

        // GetSelectionListByDriver
        [Fact]
        public async Task GetSelectionListByDriver_GivenNegativeOwnerId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service
                .Invoking(x => x.GetSelectionListByDriver(carId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetSelectionListByDriver_GivenValidId_ShouldReturnListOfCarSelectViewModel()
        {
            // arrange
            int ownerId = 1;

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Car, CarSelectViewModel>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(source => source.Brand + " " + source.CarModel + " (" + source.Year + ")"))));

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();

            mockRepository.Setup(x => x.GetAll())
                .Returns((new List<Car>()
                {
                    new Car()
                    {
                        Id = 1,
                        OwnerId = 1,
                    },
                    new Car()
                    {
                        Id = 2,
                        OwnerId = 1
                    },
                    new Car()
                    {
                        Id = 3,
                        OwnerId = 2
                    }
                }).AsQueryable<Car>());

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            int result = service.GetSelectionListByDriver(ownerId).Count;

            // assert
            result.Should().Be(2);
        }

        // GetById
        [Fact]
        public async Task GetById_GivenNegativeCarId_ShouldThrowNegativeIntException()
        {
            // arrange
            int carId = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service
                .Invoking(x => x.GetById(carId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetById_GivenNotExistingId_ShouldReturnNull()
        {
            // arrange
            int carId = 1;
            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            Car result = service.GetById(carId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetById_GivenValidId_ShouldReturnCar()
        {
            // arrange
            int carId = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetById(1))
                .Returns(new Car() { Id = 1 });

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            Car result = service.GetById(carId);

            // assert
            result.Should().Equals(new Car() { Id = 1 });
        }

        // GetAllCarsIndex
        [Fact]
        public async Task GetAllCarsIndex_GivenNullUserId_ShouldReturnNull()
        {
            // arrange
            CarsService service = new CarsService(null, null);

            // act
            var result = service.GetAllCarsIndex(null);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllCarsIndex_GivenNegativeUserId_ShouldThrowNegativeIntException()
        {
            // arrange
            int userId = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service.Invoking(x => x.GetAllCarsIndex(userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetAllCarsIndex_GivenValidUserId_ShouldReturnListOfCarIndexViewModel()
        {
            // arrange
            int userId = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<Car>()
                {
                    new Car() { Id = 1, OwnerId = 1 },
                    new Car() { Id = 2, OwnerId = 1 },
                    new Car() { Id = 3, OwnerId = 2 }
                }.AsQueryable());

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Car, CarIndexViewModel>()));

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            int result = service.GetAllCarsIndex(userId).Count();

            // assert
            result.Should().Be(2);
        }

        // GetAllAdmin
        [Fact]
        public async Task GetAllAdmin_ShouldReturnListOfCarAdminIndexViewModel()
        {
            // arrange
            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<Car>()
                {
                    new Car() { Id = 1 },
                    new Car() { Id = 2 },
                    new Car() { Id = 3 }
                }.AsQueryable());

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Car, CarAdminIndexViewModel>()));

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            int result = service.GetAllAdmin().Count();

            // assert
            result.Should().Be(3);
        }


        // CarAdminDetailsViewModel
        [Fact]
        public async Task GetDetailsAdmin_GivenNegativeId_ShouldThrowNegativeInException()
        {
            // arrange
            int id = -1;
            CarsService service = new CarsService(null, null);

            // act
            // assert
            service.Invoking(x => x.GetAllCarsIndex(id))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task GetDetailsAdmin_GivenNotExisingId_ShouldReturnNull()
        {
            // arrange
            int id = 3;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns<Car>(null);

            CarsService service = new CarsService(mockRepository.Object, null);

            // act
            var result = service.GetDetailsAdmin(id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetDetailsAdmin_GivenValidId_ShouldReturnCarAdminDetailsViewModel()
        {
            // arrange
            int id = 1;

            Mock<IDbRepository<Car>> mockRepository = new Mock<IDbRepository<Car>>();
            mockRepository.Setup(x => x.GetByIdQueryable(It.IsAny<int>()))
                .Returns(new List<Car>()
                {
                    new Car()
                    {
                        Id = 1,
                        Brand = "Brand 1",
                        CarModel = "Model 1"
                    }
                }.AsQueryable());

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.CreateMap<Car, CarAdminDetailsViewModel>()));

            CarsService service = new CarsService(mockRepository.Object, mapper);

            // act
            var result = service.GetDetailsAdmin(id);

            // assert
            result.Should().Equals(new CarAdminDetailsViewModel()
            {
                Id = 1,
                Brand = "Brand 1",
                CarModel = "Model 1"
            });
        }
    }
}
