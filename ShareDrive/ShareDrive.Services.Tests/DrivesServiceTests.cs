namespace ShareDrive.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using FluentAssertions;
    using Moq;
    using Xunit;

    using ShareDrive.Common;
    using ShareDrive.Exceptions;
    using ShareDrive.Models;
    using ShareDrive.Services.Contracts;
    using ShareDrive.Services.Models;
    using ShareDrive.ViewModels.Admin.Drive;
    using ShareDrive.ViewModels.Drive;
    
    public class DrivesServiceTests
    {
        // GetAll
        [Fact]
        public void GetAll_ShouldReturnAllDrivesAsDriveIndexViewModel()
        {
            // arrange
            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetAll())
                .Returns(new List<Drive>()
                {
                    new Drive()
                    {
                        Id = 1
                    },
                    new Drive()
                    {
                        Id = 2
                    }
                }.AsQueryable());

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveIndexViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(source => source.DateTime.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetAll();

            // assert
            result.Count().Should().Be(2);
        }

        // GetAllAdmin
        [Fact]
        public void GetAllAdmin_ShouldReturnAllDrivesAsAdminDriveIndexViewModel()
        {
            // arrange
            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetAll())
                .Returns(new List<Drive>()
                {
                    new Drive()
                    {
                        Id = 1
                    },
                    new Drive()
                    {
                        Id = 2
                    }
                }.AsQueryable());

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveAdminIndexViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(source => source.DateTime.ToString("dd/MM/yyyy HH:mm")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetAllAdmin().Count();

            // assert
            result.Should().Be(2);
        }

        //GetById
        [Fact]
        public void GetById_GivenNegativeId_ShouldThrowMegativeIntException()
        {
            // arrange
            int id = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetById(id))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetById_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int id = 10;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetById(id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetById_GivenValidId_ShouldReturnValidDrive()
        {
            // arrange
            int id = 1;
            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns(new Drive()
                {
                    Id = id
                });

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetById(id);

            // assert
            result.Should().Equals(new Drive() { Id = id });
        }

        // Create
        [Fact]
        public void Create_GivenNullModel_ShouldThrowNullObjectException()
        {
            // arrange
            var data = new ParsedDriveCreateData();
            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(null, data);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void Create_GivenNullData_ShouldThrowNullObjectException()
        {
            // arrange
            var model = new DriveCreateEditViewModel();
            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, null);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void Create_GivenDataWithNullFrom_ShouldReturnNull()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveCreateData data = new ParsedDriveCreateData()
            {
                From = null,
                To = new City(),
                Car = new Car(),
                Date = new DateTime()
            };

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, data);

            // assert
            action.Should().Equals(Task.FromResult<Drive>(null));
        }

        [Fact]
        public void Create_GivenDataWithNullTo_ShouldReturnNull()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveCreateData data = new ParsedDriveCreateData()
            {
                From = new City(),
                To = null,
                Car = new Car(),
                Date = new DateTime(),
                Driver = new ApplicationUser()
            };

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, data);

            // assert
            action.Should().Equals(Task.FromResult<Drive>(null));
        }

        [Fact]
        public void Create_GivenDataWithNullCar_ShouldReturnNull()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveCreateData data = new ParsedDriveCreateData()
            {
                From = new City(),
                To = new City(),
                Car = null,
                Date = new DateTime(),
                Driver = new ApplicationUser()
            };

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, data);

            // assert
            action.Should().Equals(Task.FromResult<Drive>(null));
        }

        [Fact]
        public void Create_GivenDataWithNullDriver_ShouldReturnNull()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveCreateData data = new ParsedDriveCreateData()
            {
                From = new City(),
                To = new City(),
                Car = new Car(),
                Date = new DateTime(),
                Driver = null
            };

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, data);

            // assert
            action.Should().Equals(Task.FromResult<Drive>(null));
        }

        [Fact]
        public void Create_GivenValidModelAndData_ShouldReturnCompletedTask()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveCreateData data = new ParsedDriveCreateData()
            {
                From = new City(),
                To = new City(),
                Car = new Car(),
                Date = new DateTime(),
                Driver = new ApplicationUser()
            };

            Mock<IDbRepository<Drive>> mockRepository = new Mock<Common.IDbRepository<Drive>>();
            mockRepository.Setup(m => m.CreateAsync(It.IsAny<Drive>()))
                .Returns(Task.FromResult(new Drive()));

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.CreateMap<DriveCreateEditViewModel, Drive>()
                .ForMember(dest => dest.From, opt => opt.Ignore())
                .ForMember(dest => dest.To, opt => opt.Ignore())
                .ForMember(dest => dest.Car, opt => opt.Ignore())
                .ForMember(dest => dest.Driver, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.Ignore())));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            Func<Task> action = async () => await service.CreateAsync(model, data);

            // assert
            action.Should().NotThrow();
        }

        // GetEditModelById
        [Fact]
        public void GetEditModelById_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int id = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetEditModelById(id))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetEditModelById_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int id = 10;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetEditModelById(id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetEditModelById_GivenValidId_ShouldReturnValidDriveCreateEditViewModel()
        {
            // arrange
            int id = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns(new Drive() { Id = id });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveCreateEditViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.Cars, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime.ToString("o")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetEditModelById(id);

            // assert
            result.Should().Equals(new DriveCreateEditViewModel() { Id = id });
        }

        // Update
        [Fact]
        public void Update_GivenNullModel_ShouldThrowNullObjectException()
        {
            // arrange
            DriveCreateEditViewModel model = null;
            var data = new ParsedDriveUpdateData();
            int driveId = 1;

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.UpdateAsync(model, data, driveId);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void Update_GivenNullData_ShouldThrowNullObjectException()
        {
            // arrange
            var model = new DriveCreateEditViewModel();
            ParsedDriveUpdateData data = null;
            int driveId = 1;

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.UpdateAsync(model, data, driveId);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void Update_GivenNegativeDriveId_ShouldThrowNegativeIntException()
        {
            // arrange
            var model = new DriveCreateEditViewModel();
            var data = new ParsedDriveUpdateData();
            int driveId = -1;

            IDrivesService service = new DrivesService(null, null, null);

            // act
            Func<Task> action = async () => await service.UpdateAsync(model, data, driveId);

            // assert
            action.Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public async Task Update_GivenNonExistingDriveId_ShouldReturnFalse()
        {
            // arrange
            var model = new DriveCreateEditViewModel();
            var data = new ParsedDriveUpdateData();
            int driveId = 10;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            bool result = await service.UpdateAsync(model, data, driveId);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Update_GivenDataWithNullFrom_ShouldReturnFalse()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveUpdateData data = new ParsedDriveUpdateData()
            {
                From = null,
                To = new City(),
                Car = new Car(),
                Date = new DateTime()
            };
            int driveId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = await service.UpdateAsync(model, data, driveId);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Update_GivenDataWithNullTo_ShouldReturnFalse()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveUpdateData data = new ParsedDriveUpdateData()
            {
                From = new City(),
                To = null,
                Car = new Car(),
                Date = new DateTime()
            };
            int driveId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = await service.UpdateAsync(model, data, driveId);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Update_GivenDataWithNullCar_ShouldReturnFalse()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveUpdateData data = new ParsedDriveUpdateData()
            {
                From = new City(),
                To = new City(),
                Car = null,
                Date = new DateTime()
            };
            int driveId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = await service.UpdateAsync(model, data, driveId);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Update_GivenValidModelDataAndDriveId_ShouldReturnTrue()
        {
            // arrange
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();
            ParsedDriveUpdateData data = new ParsedDriveUpdateData()
            {
                From = new City(),
                To = new City(),
                Car = new Car(),
                Date = new DateTime()
            };
            int driveId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.CreateMap<Drive, DriveCreateEditViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.Cars, opt => opt.Ignore())
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime.ToString("o")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = await service.UpdateAsync(model, data, driveId);

            // assert
            result.Should().BeTrue();
        }

        // GetDeleteModelById
        [Fact]
        public void GetDeleteModelById_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int id = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetDeleteModelById(id))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetDeleteModelById_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int id = 10;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetDeleteModelById(id);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetDeleteModelById_GivenValidId_ShouldReturnValidDriveDeleteViewModel()
        {
            // arrange
            int id = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns(new Drive() { Id = id });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveDeleteViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.DateTime, opt => opt.MapFrom(source => source.DateTime.ToString("o")))
                .ForMember(dest => dest.Car, opt => opt.MapFrom(source => source.Car.Brand + " " + source.Car.CarModel + " (" + source.Car.Year + ")"))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetDeleteModelById(id);

            // assert
            result.Should().Equals(new DriveDeleteViewModel() { Id = id });
        }

        // Delete
        [Fact]
        public void Delete_GivenNegativeId_ShouldThrowNegativeIntException()
        {
            // arrange
            int id = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.Delete(id))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void Delete_GivenNonExistingId_ShouldReturnFalse()
        {
            // arrange
            int id = 10;
            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            bool result = service.Delete(id);

            // assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Delete_GivenValidId_ShouldReturnTrue()
        {
            // arrange
            int id = 1;
            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();

            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == id)))
                .Returns(new Drive() { Id = id });

            mockRepository.Setup(m => m.Delete(It.Is<Drive>(d => d.Id == id)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            bool result = service.Delete(id);

            // assert
            result.Should().BeTrue();
        }

        // GetDetailsModel
        [Fact]
        public void GetDetailsModel_GivenNegativeDriveId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            int userId = 1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetDetailsModel(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetDetailsModel_GivenNegativeUserId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = 1;
            int userId = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetDetailsModel(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetDetailsModel_GivenNonExistingId_ShouldReturnNull()
        {
            // arrange
            int driveId = 10;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetDetailsModel(driveId, userId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetDetailsModel_GivenValidDriveIdAndUserIsDriver_ShouldReturnValidDetailsModelWithIsOwnerTrue()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId, DriverId = userId });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveDetailsViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(source => source.DateTime.ToString("dd MM yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetDetailsModel(driveId, userId);

            // assert
            result.Should().Equals(new DriveDetailsViewModel() { Id = driveId, UserIsOwner = true });
        }

        [Fact]
        public void GetDetailsModel_GivenValidDriveIdAndUserIsNotDriver_ShouldReturnValidDetailsModelWithIsOwnerFalse()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId, DriverId = 2 });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveDetailsViewModel>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(source => source.From.Name))
                .ForMember(dest => dest.To, opt => opt.MapFrom(source => source.To.Name))
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.MapFrom(source => source.DeclaredSeats - source.DrivesPassengers.Count))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(source => source.DateTime.ToString("dd MM yyyy")))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(source => source.DateTime.ToString("HH:mm")))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetDetailsModel(driveId, userId);

            // assert
            result.Should().Equals(new DriveDetailsViewModel() { Id = driveId, UserIsOwner = false });
        }

        // GetDetailsAdminModel
        [Fact]
        public void GetDetailsAdminModel_GivenNegativeDriveId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetDetailsAdminModel(driveId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetDetailsAdminModel_GivenNonExistingDriveId_ShouldReturnNull()
        {
            // arrange
            int driveId = 10;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.GetDetailsAdminModel(driveId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetDetailsAdminModel_GivenValidDriveId_ShouldReturnValidDriveAdminDetailsViewModel()
        {
            // arrange
            int driveId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
                cfg.CreateMap<Drive, DriveAdminDetailsViewModel>()
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
                    opt => opt.MapFrom(source => source.Car.Brand + " " + source.Car.CarModel + " " + source.Car.Year))));

            IDrivesService service = new DrivesService(mockRepository.Object, mapper, null);

            // act
            var result = service.GetDetailsAdminModel(driveId);

            // assert
            result.Should().Equals(new DriveAdminDetailsViewModel() { Id = driveId });
        }

        // ReserveSeat
        [Fact]
        public void ReserveSeat_GivenNegativeDriveId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            int userId = 1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.ReserveSeat(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void ReserveSeat_GivenNegativeUserId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = 1;
            int userId = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.ReserveSeat(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void ReserveSeat_GivenNonExistingDriveId_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "This drive does not exist."));
        }

        [Fact]
        public void ReserveSeat_GivenNonExistingUserId_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            Mock <IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(false);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "This user does not exist."));
        }

        [Fact]
        public void ReserveSeat_GivenUserIdWithReservation_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive()
                {
                    Id = driveId,
                    DrivesPassengers = new List<DrivesPassengers>()
                    {
                        new DrivesPassengers() { DriveId = driveId, PassengerId = userId }
                    }
                });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "You already have a reservation for the drive."));
        }

        [Fact]
        public void ReserveSeat_GivenDriveIdWithNoFreeSeats_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive()
                {
                    Id = driveId,
                    DeclaredSeats = 1,
                    DrivesPassengers = new List<DrivesPassengers>()
                    {
                        new DrivesPassengers()
                    }
                });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "All seats for the drive are taken."));
        }

        [Fact]
        public void ReserveSeat_GivenValidDriveIdAndValidUserId_ShouldReturnTrueAndSuccessMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive()
                {
                    Id = driveId,
                    DeclaredSeats = 2,
                    DrivesPassengers = new List<DrivesPassengers>()
                    {
                        new DrivesPassengers()
                    }
                });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(true, "You made a reservation! Have a safe drive."));
        }

        // CancelReservation
        [Fact]
        public void CancelReservation_GivenNegativeDriveId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            int userId = 1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.CancelReservation(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void CancelReservation_GivenNegativeUserId_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = 1;
            int userId = -1;
            IDrivesService service = new DrivesService(null, null, null);

            // act
            // assert
            service.Invoking(s => s.CancelReservation(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void CancelReservation_GivenNonExistingDriveId_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns<Drive>(null);

            IDrivesService service = new DrivesService(mockRepository.Object, null, null);

            // act
            var result = service.CancelReservation(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "This drive does not exist."));
        }

        [Fact]
        public void CancelReservation_GivenNonExistingUserId_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive() { Id = driveId });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(false);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.CancelReservation(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "This user does not exist."));
        }

        [Fact]
        public void CancelReservation_GivenUserIdWithoutReservation_ShouldReturnFalseAndErrorMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive()
                {
                    Id = driveId,
                    DrivesPassengers = new List<DrivesPassengers>()
                    {
                        new DrivesPassengers() { DriveId = driveId, PassengerId = 11 }
                    }
                });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.ReserveSeat(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(false, "You don't have a reservation for that drive."));
        }

        [Fact]
        public void CancelReservation_GivenValidDriveIdAndValidUserId_ShouldReturnTrueAndSuccessMessage()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDbRepository<Drive>> mockRepository = new Mock<IDbRepository<Drive>>();
            mockRepository.Setup(m => m.GetById(It.Is<int>(i => i == driveId)))
                .Returns(new Drive()
                {
                    Id = driveId,
                    DeclaredSeats = 1,
                    DrivesPassengers = new List<DrivesPassengers>()
                    {
                        new DrivesPassengers() { DriveId = driveId, PassengerId = userId }
                    }
                });

            Mock<IUsersService> mockUsersService = new Mock<IUsersService>();
            mockUsersService.Setup(m => m.CheckIfUserExists(It.Is<int>(i => i == userId)))
                .Returns(true);

            IDrivesService service = new DrivesService(mockRepository.Object, null, mockUsersService.Object);

            // act
            var result = service.CancelReservation(driveId, userId);

            // assert
            result.Should().Equals(new KeyValuePair<bool, string>(true, "Your registration has been canceled."));
        }
    }
}
