using FluentAssertions;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using HospitalLibrary.Core.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Services
{
    public class TestHealthInfoService
    {
        private readonly List<HealthInfo> healthInfos;
        private Mock<IHealthInfoRepository> healthInfoRepositoryMock;
        private HealthInfoService healthInfoService;

        public TestHealthInfoService()
        {
            healthInfos = new List<HealthInfo>
            {
                new HealthInfo
                {
                    Id = 1,
                    Date = new DateTime(2023, 1, 28),
                    OwnersJmbg = 1234567890,
                    UpperBloodPreassure = 120,
                    LowerBloodPreassure = 80,
                    SugarLevel = 90,
                    FatPercentage = 20.5,
                    Weight = 70.0,
                    LastMenstruation = new DateTime(1900, 1, 1)

                },
                new HealthInfo
                {
                    Id = 2,
                    Date = new DateTime(2023, 4, 20),
                    OwnersJmbg = 1234567890,
                    UpperBloodPreassure = 130,
                    LowerBloodPreassure = 85,
                    SugarLevel = 95,
                    FatPercentage = 22.0,
                    Weight = 72.5,
                    LastMenstruation = new DateTime(1900, 1, 1)
                },

                new HealthInfo
                {
                    Id = 3,
                    Date = new DateTime(2023, 9, 5),
                    OwnersJmbg = 1234567890,
                    UpperBloodPreassure = 140,
                    LowerBloodPreassure = 75,
                    SugarLevel = 88,
                    FatPercentage = 18.5,
                    Weight = 68.8,
                    LastMenstruation = new DateTime(1900, 1, 1)
                }
            };

            healthInfoRepositoryMock = new Mock<IHealthInfoRepository>();
            healthInfoService = new HealthInfoService(healthInfoRepositoryMock.Object);
        }

        [Fact]
        public void GetAllHealthInfo_ReturnAllHealthInfo()
        {
            // Arrange
            healthInfoRepositoryMock.Setup(repo => repo.GetAll()).Returns(healthInfos);

            // Act
            var result = healthInfoService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthInfos);
        }

        [Fact]
        public void GetAllHealthInfo_ReturnsEmptyListWhenNoHealthInfoExist()
        {
            // Arrange
            healthInfoRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<HealthInfo>());

            // Act
            var result = healthInfoService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetHealthInfoById_ReturnsHealthInfoWithValidId()
        {
            // Arrange
            var healthInfoIdToFind = 1;
            var healthInfoToReturn = new HealthInfo { Id = healthInfoIdToFind, Date = new DateTime(2023, 1, 28) };
            healthInfoRepositoryMock.Setup(repo => repo.GetById(healthInfoIdToFind)).Returns(healthInfoToReturn);

            // Act
            var result = healthInfoService.GetById(healthInfoIdToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthInfoToReturn);
        }

        [Fact]
        public void GetHealthInfoById_ReturnsNullForInvalidId()
        {
            // Arrange
            var invalidHealthInfoId = 999;
            healthInfoRepositoryMock.Setup(repo => repo.GetById(invalidHealthInfoId)).Returns((HealthInfo)null);

            // Act
            var result = healthInfoService.GetById(invalidHealthInfoId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void CreateHealthInfo_AddsHealthInfoToRepository()
        {
            // Arrange
            var newHealthInfo = new HealthInfo
            {
                Date = new DateTime(2023, 11, 10),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 125,
                LowerBloodPreassure = 85,
                SugarLevel = 92,
                FatPercentage = 21.0,
                Weight = 71.5,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            // Act
            healthInfoService.Create(newHealthInfo);

            // Assert
            healthInfoRepositoryMock.Verify(repo => repo.Create(newHealthInfo), Times.Once);
        }

        [Fact]
        public void UpdateHealthInfo_UpdatesHealthInfoInRepository()
        {
            // Arrange
            var healthInfoToUpdate = new HealthInfo
            {
                Id = 1,
                Date = new DateTime(2023, 12, 5),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 135,
                LowerBloodPreassure = 88,
                SugarLevel = 97,
                FatPercentage = 23.0,
                Weight = 73.2,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            // Act
            healthInfoService.Update(healthInfoToUpdate);

            // Assert
            healthInfoRepositoryMock.Verify(repo => repo.Update(healthInfoToUpdate), Times.Once);
        }

        [Fact]
        public void DeleteHealthInfo_DeletesHealthInfoFromRepository()
        {
            // Arrange
            var healthInfoToDelete = new HealthInfo
            {
                Id = 1,
                Date = new DateTime(2023, 1, 28),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 120,
                LowerBloodPreassure = 80,
                SugarLevel = 90,
                FatPercentage = 20.5,
                Weight = 70.0,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            // Act
            healthInfoService.Delete(healthInfoToDelete);

            // Assert
            healthInfoRepositoryMock.Verify(repo => repo.Delete(healthInfoToDelete), Times.Once);
        }

        [Fact]
        public void GetHealthInfosByOwnersJmbg_ReturnsHealthInfosWithMatchingOwnersJmbg()
        {
            // Arrange
            var targetOwnersJmbg = 1234567890;
            var healthInfosWithMatchingJmbg = healthInfos.Where(h => h.OwnersJmbg == targetOwnersJmbg);
            healthInfoRepositoryMock.Setup(repo => repo.GetAllOfOwner(targetOwnersJmbg)).Returns(healthInfosWithMatchingJmbg);

            // Act
            var result = healthInfoService.GetAllOfOwner(targetOwnersJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(healthInfosWithMatchingJmbg);
        }

        [Fact]
        public void GetHealthInfosByOwnersJmbg_ReturnsEmptyListWhenNoMatchingHealthInfosExist()
        {
            // Arrange
            var targetOwnersJmbg = 999999999;
            healthInfoRepositoryMock.Setup(repo => repo.GetAllOfOwner(targetOwnersJmbg)).Returns(new List<HealthInfo>());

            // Act
            var result = healthInfoService.GetAllOfOwner(targetOwnersJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }
    }
}
