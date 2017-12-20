namespace ShareDrive.Services.Tests
{
    using AutoMapper;
    using FluentAssertions;
    using Moq;
    using ShareDrive.Common;
    using ShareDrive.Exceptions;
    using ShareDrive.Models;
    using ShareDrive.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class CitiesServiceTests
    {
        // GetOrCreateAsync
        [Fact]
        public void GetOrCreateAsync_GivenNullString_ShouldThrowStringNullOrEmptyException()
        {
            // arrange
            ICitiesService service = new CitiesService(null);

            // act
            Func<Task> action = () => service.GetOrCreateAsync(null);

            // assert
            action.Should().ThrowExactly<StringNullOrEmptyException>();
        }

        [Fact]
        public void GetOrCreateAsync_GivenEmptyString_ShouldThrowStringNullOrEmptyException()
        {
            // arrange
            ICitiesService service = new CitiesService(null);

            // act
            Func<Task> action = () => service.GetOrCreateAsync("");

            // assert
            action.Should().ThrowExactly<StringNullOrEmptyException>();
        }

        [Fact]
        public void GetOrCreate_GivenNonExistingName_ShouldReturnCompletedTask()
        {
            // arrange
            string name = "test";

            Mock<IDbRepository<City>> mockRepository = new Mock<IDbRepository<City>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<City>().AsQueryable());

            mockRepository
                .Setup(x => x.CreateAsync(It.Is<City>(city => city.Name == "test")))
                .Returns(Task.FromResult(new City() { Name = "test" }));

            ICitiesService service = new CitiesService(mockRepository.Object);

            // act
            Func<Task> action = async () => await service.GetOrCreateAsync(name);

            // assert
            action.Should().NotThrow();
        }

        [Fact]
        public void GetOrCreateAsync_GivenNonExistingName_ShouldReturnNewCityWithThatName()
        {
            // arrange
            string name = "test";

            Mock<IDbRepository<City>> mockRepository = new Mock<IDbRepository<City>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<City>().AsQueryable());

            mockRepository
                .Setup(x => x.CreateAsync(It.Is<City>(city => city.Name == "test")))
                .Returns(Task.FromResult(new City() { Name = "test" }));

            ICitiesService service = new CitiesService(mockRepository.Object);

            // act
            Func<Task> action = async () => await service.GetOrCreateAsync(name);

            // assert
            action.Should().Equals(new City() { Name = name });
        }

        [Fact]
        public void GetOrCreate_GivenExistingName_ShouldReturnCompletedTask()
        {
            // arrange
            string name = "test";

            Mock<IDbRepository<City>> mockRepository = new Mock<IDbRepository<City>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<City>()
                {
                    new City() { Id = 1, Name = "test" },
                    new City() { Id = 2, Name = "another test" }
                }.AsQueryable());

            mockRepository
                .Setup(x => x.CreateAsync(It.Is<City>(city => city.Name == "test")))
                .Returns(Task.FromResult(new City() { Id = 1, Name = "test" }));

            ICitiesService service = new CitiesService(mockRepository.Object);

            // act
            Func<Task> action = async () => await service.GetOrCreateAsync(name);

            // assert
            action.Should().NotThrow();
        }

        [Fact]
        public void GetOrCreate_GivenExistingName_ShouldReturnCityWithThatName()
        {
            // arrange
            string name = "test";

            Mock<IDbRepository<City>> mockRepository = new Mock<IDbRepository<City>>();
            mockRepository.Setup(x => x.GetAll())
                .Returns(new List<City>()
                {
                    new City() { Id = 1, Name = "test" },
                    new City() { Id = 2, Name = "another test" }
                }.AsQueryable());

            mockRepository
                .Setup(x => x.CreateAsync(It.Is<City>(city => city.Name == "test")))
                .Returns(Task.FromResult(new City() { Id = 1, Name = "test" }));

            ICitiesService service = new CitiesService(mockRepository.Object);

            // act
            Func<Task> action = async () => await service.GetOrCreateAsync(name);

            // assert
            action.Should().Equals(new City() { Id = 1, Name = name });
        }
    }
}
