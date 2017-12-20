namespace ShareDrive.Services.Tests
{
    using System;
    using System.Threading.Tasks;

    using FluentAssertions;
    using Xunit;

    using ShareDrive.Exceptions;
    using ShareDrive.Services.Contracts;
    using Moq;
    using System.Collections.Generic;
    using ShareDrive.ViewModels.Car;
    using ShareDrive.ViewModels.Drive;
    using ShareDrive.Models;
    using AutoMapper;
    using ShareDrive.Services.Models;

    public class DriveHelperServiceTests
    {
        // GetCreateViewModel
        [Fact]
        public void GetCreateViewModel_GivenNegativeUserId_ShouldThrowNegativeIntException()
        {
            // arrange
            int userId = -1;

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            // assert
            service
                .Invoking(x => x.GetCreateViewModel(userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetCreateViewModel_GivenNonExistingUserId_ShouldReturnModelWithEmptyCarsSelectList()
        {
            // arrange
            int userId = 1;

            Mock<ICarsService> carsService = new Mock<ICarsService>();
            carsService.Setup(x => x.GetSelectionListByDriver(It.IsAny<int>()))
                .Returns(new List<CarSelectViewModel>());

            IDriveHelperService service = new DriveHelperService(null, null, carsService.Object, null, null);

            // act
            DriveCreateEditViewModel result = service.GetCreateViewModel(userId);

            // assert
            result.Should().Equals(new DriveCreateEditViewModel() { Cars = new List<CarSelectViewModel>() });
        }

        [Fact]
        public void GetCreateViewModel_GivenValidUserIdWithExistingCars_ShouldReturnModelWithAllCarsOfThatUser()
        {
            // arrange
            int userId = 1;

            Mock<ICarsService> carsService = new Mock<ICarsService>();
            carsService.Setup(x => x.GetSelectionListByDriver(It.IsAny<int>()))
                .Returns(new List<CarSelectViewModel>()
                {
                    new CarSelectViewModel() { Id = 1, Name = "TestCar1" },
                    new CarSelectViewModel() { Id = 2, Name = "TestCar2" }
                });

            IDriveHelperService service = new DriveHelperService(null, null, carsService.Object, null, null);

            // act
            DriveCreateEditViewModel result = service.GetCreateViewModel(userId);

            // assert
            result.Should().Equals(new DriveCreateEditViewModel()
            {
                Cars = new List<CarSelectViewModel>()
                {
                    new CarSelectViewModel() { Id = 1, Name = "TestCar1" },
                    new CarSelectViewModel() { Id = 2, Name = "TestCar2" }
                }
            });
        }

        // ProcessCreateDriveAsync
        [Fact]
        public void ProcessCreateDriveAsync_GivenNullDriveCreateEditViewModel_ShouldThrowNullObjectException()
        {
            // arrange
            int userId = 1;

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(null, userId);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void ProcessCreateDriveAsync_GivenNegativeUserIs_ShouldThrowNegativeIntException()
        {
            // arrange
            int userId = -1;
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(model, userId);

            // assert
            action.Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void ProcessCreateDriveAsync_GivenModelWithInvalidDateFormat_ShouldThrowInvalidDateFormatException()
        {
            // arrange
            int userId = 1;

            string from = "TestFrom";
            string to = "TestTo";
            string date = "11/11/2011";
            int carId = 1;

            DriveCreateEditViewModel model = new DriveCreateEditViewModel()
            {
                From = from,
                To = to,
                DateTime = date,
            };

            Mock<ICitiesService> citiesServiceMock = new Mock<ICitiesService>();
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == from)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = from }));
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == to)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = to }));

            Mock<ICarsService> carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.GetById(It.Is<int>(i => i == carId)))
                .Returns(new Car() { Id = carId });
            
            IDriveHelperService service = new DriveHelperService(
                null,
                citiesServiceMock.Object,
                carsServiceMock.Object, null, null);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(model, userId);

            // assert
            action.Should().ThrowExactly<InvalidDateFormatException>();
        }

        [Fact]
        public void ProcessCreateDriveAsync_GivenValidModelAndUserId_ShouldReturnCompletedTask()
        {
            // arrange
            int userId = 1;

            string from = "TestFrom";
            string to = "TestTo";
            string date = "11/11/2011 - 11:11";
            decimal price = 10;
            int declaredSeats = 3;
            string locationToPick = "TestLocationToPick";
            string locationToArrive = "TestLocationToArrive";
            int carId = 1;

            DriveCreateEditViewModel model = new DriveCreateEditViewModel()
            {
                From = from,
                To = to,
                DateTime = date,
                Price = price,
                DeclaredSeats = declaredSeats,
                LocationToPick = locationToPick,
                LocationToArrive = locationToArrive,
                CarId = carId
            };

            Mock<ICitiesService> citiesServiceMock = new Mock<ICitiesService>();
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == from)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = from }));
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == to)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = to }));

            Mock<ICarsService> carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.GetById(It.Is<int>(i => i == carId)))
                .Returns(new Car() { Id = carId });

            Mock<IUsersService> usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(x => x.GetById(It.Is<int>(i => i == userId)))
                .Returns(new ApplicationUser() { Id = userId });

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<BaseParsedDriveData, ParsedDriveCreateData>()));
              
            Mock<IDrivesService> drivesServiceMock = new Mock<IDrivesService>();
            drivesServiceMock.Setup(x =>
                x.CreateAsync(It.Is<DriveCreateEditViewModel>(d => d.From == from
                    && d.To == to
                    && d.Price == price
                    && d.DateTime == date
                    && d.DeclaredSeats == declaredSeats
                    && d.CarId == carId
                    && d.LocationToPick == locationToPick
                    && d.LocationToArrive == locationToArrive),
                    It.Is<ParsedDriveCreateData>(p => p.From.Name == from
                    && p.To.Name == to
                    && p.Date.ToString("dd/MM/yyyy - HH:mm") == date
                    && p.Driver.Id == userId)))
                    .Returns(Task.CompletedTask);

            IDriveHelperService service = new DriveHelperService(
                drivesServiceMock.Object,
                citiesServiceMock.Object,
                carsServiceMock.Object,
                usersServiceMock.Object,
                mapper);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(model, userId);

            // assert
            action.Should().NotThrow();
        }

        // GetEditViewModel
        [Fact]
        public void GetEditViewModel_GivenNegativeDriveIs_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            int userId = 1;

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetEditViewModel(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetEditViewModel_GivenNegativeUserIs_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = 1;
            int userId = -1;

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            // assert
            service.Invoking(s => s.GetEditViewModel(driveId, userId))
                .Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void GetEditViewModel_GivenNonExistingDriveId_ShouldReturnNull()
        {
            // arrange
            int driveId = 3;
            int userId = 1;

            Mock<IDrivesService> mockDrivesService = new Mock<IDrivesService>();
            mockDrivesService.Setup(m => m.GetEditModelById(It.Is<int>(i => i == driveId)))
                .Returns<DriveCreateEditViewModel>(null);

            IDriveHelperService service = new DriveHelperService(mockDrivesService.Object, null, null, null, null);

            // act
            var result = service.GetEditViewModel(driveId, userId);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetEditViewModel_GivenNonExistingUserId_ShouldReturnValidDriveCreateEditViewModelWithEmptyCarsSelectList()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDrivesService> mockDrivesService = new Mock<IDrivesService>();
            mockDrivesService.Setup(m => m.GetEditModelById(It.Is<int>(i => i == driveId)))
                .Returns(new DriveCreateEditViewModel()
                {
                    Id = 1
                });

            Mock<ICarsService> mockCarsService = new Mock<ICarsService>();
            mockCarsService.Setup(m => m.GetSelectionListByDriver(It.Is<int>(i => i == userId)))
                .Returns(new List<CarSelectViewModel>());

            IDriveHelperService service = new DriveHelperService(mockDrivesService.Object, null, mockCarsService.Object, null, null);

            // act
            var result = service.GetEditViewModel(driveId, userId).Cars.Count;

            // assert
            result.Should().Be(0);
        }

        [Fact]
        public void GetEditViewModel_GivenValidDriveIdAndValidUserIdWithOneCar_ShouldReturnValidDriveCreateEditViewModelWithNonEmptyCarsSelectListWithOneCar()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDrivesService> mockDrivesService = new Mock<IDrivesService>();
            mockDrivesService.Setup(m => m.GetEditModelById(It.Is<int>(i => i == driveId)))
                .Returns(new DriveCreateEditViewModel()
                {
                    Id = 1
                });

            Mock<ICarsService> mockCarsService = new Mock<ICarsService>();
            mockCarsService.Setup(m => m.GetSelectionListByDriver(It.Is<int>(i => i == userId)))
                .Returns(new List<CarSelectViewModel>()
                {
                    new CarSelectViewModel()
                });

            IDriveHelperService service = new DriveHelperService(mockDrivesService.Object, null, mockCarsService.Object, null, null);

            // act
            var result = service.GetEditViewModel(driveId, userId).Cars.Count;

            // assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetEditViewModel_GivenValidDriveIdAndValidUserIdWithMoreThanOneCar_ShouldReturnValidDriveCreateEditViewModelWithNonEmptyCarsSelectListWithCountUserCarsPlusOne()
        {
            // arrange
            int driveId = 1;
            int userId = 1;

            Mock<IDrivesService> mockDrivesService = new Mock<IDrivesService>();
            mockDrivesService.Setup(m => m.GetEditModelById(It.Is<int>(i => i == driveId)))
                .Returns(new DriveCreateEditViewModel()
                {
                    Id = 1
                });

            Mock<ICarsService> mockCarsService = new Mock<ICarsService>();
            mockCarsService.Setup(m => m.GetSelectionListByDriver(It.Is<int>(i => i == userId)))
                .Returns(new List<CarSelectViewModel>()
                {
                    new CarSelectViewModel(),
                    new CarSelectViewModel(),
                    new CarSelectViewModel()
                });

            IDriveHelperService service = new DriveHelperService(mockDrivesService.Object, null, mockCarsService.Object, null, null);

            // act
            var result = service.GetEditViewModel(driveId, userId).Cars.Count;

            // assert
            result.Should().Be(4);
        }

        // ProcessEditDriveAsync
        [Fact]
        public void ProcessEditDriveAsync_GivenNullDriveCreateEditViewModel_ShouldThrowNullObjectException()
        {
            // arrange
            int driveId = 1;

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            Func<Task> action = () => service.ProcessEditDriveAsync(null, driveId);

            // assert
            action.Should().ThrowExactly<NullObjectException>();
        }

        [Fact]
        public void ProcessEditDriveAsync_GivenNegativeDriveIs_ShouldThrowNegativeIntException()
        {
            // arrange
            int driveId = -1;
            DriveCreateEditViewModel model = new DriveCreateEditViewModel();

            IDriveHelperService service = new DriveHelperService(null, null, null, null, null);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(model, driveId);

            // assert
            action.Should().ThrowExactly<NegativeIntException>();
        }

        [Fact]
        public void ProcessEditDriveAsync_GivenModelWithInvalidDateFormat_ShouldThrowInvalidDateFormatException()
        {
            // arrange
            int driveId = 1;

            string from = "TestFrom";
            string to = "TestTo";
            string date = "11/11/2011";
            int carId = 1;

            DriveCreateEditViewModel model = new DriveCreateEditViewModel()
            {
                From = from,
                To = to,
                DateTime = date,
            };

            Mock<ICitiesService> citiesServiceMock = new Mock<ICitiesService>();
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == from)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = from }));
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == to)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = to }));

            Mock<ICarsService> carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.GetById(It.Is<int>(i => i == carId)))
                .Returns(new Car() { Id = carId });

            IDriveHelperService service = new DriveHelperService(
                null,
                citiesServiceMock.Object,
                carsServiceMock.Object, null, null);

            // act
            Func<Task> action = () => service.ProcessCreateDriveAsync(model, driveId);

            // assert
            action.Should().ThrowExactly<InvalidDateFormatException>();
        }

        [Fact]
        public void ProcessEditDriveAsync_GivenValidModelAndDriveId_ShouldReturnCompletedTask()
        {
            // arrange
            int driveId = 1;

            string from = "TestFrom";
            string to = "TestTo";
            string date = "11/11/2011 - 11:11";
            int carId = 1;

            DriveCreateEditViewModel model = new DriveCreateEditViewModel()
            {
                From = from,
                To = to,
                DateTime = date,
                CarId = carId
            };

            Mock<ICitiesService> citiesServiceMock = new Mock<ICitiesService>();
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == from)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = from }));
            citiesServiceMock.Setup(x => x.GetOrCreateAsync(It.Is<string>(s => s == to)))
                .Returns(Task.FromResult(new City() { Id = 1, Name = to }));

            Mock<ICarsService> carsServiceMock = new Mock<ICarsService>();
            carsServiceMock.Setup(x => x.GetById(It.Is<int>(i => i == carId)))
                .Returns(new Car() { Id = carId });
            
            Mock<IDrivesService> drivesServiceMock = new Mock<IDrivesService>();
            drivesServiceMock.Setup(x =>
                x.UpdateAsync(It.Is<DriveCreateEditViewModel>(d => d.From == from
                    && d.To == to
                    && d.DateTime == date
                    && d.CarId == carId),
                    It.Is<ParsedDriveUpdateData>(p => p.From.Name == from
                    && p.To.Name == to
                    && p.Date.ToString("dd/MM/yyyy - HH:mm") == date),
                    It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            IDriveHelperService service = new DriveHelperService(
                drivesServiceMock.Object,
                citiesServiceMock.Object,
                carsServiceMock.Object,
                null,
                null);

            // act
            Func<Task> action = () => service.ProcessEditDriveAsync(model, driveId);

            // assert
            action.Should().NotThrow();
        }
    }
}
