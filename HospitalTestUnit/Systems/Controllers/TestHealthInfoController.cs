using FluentAssertions;
using HospitalAPI.Controllers;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Controllers
{
    public class TestHealthInfoController
    {
        private readonly List<HealthInfo> healthInfos;
        private Mock<IHealthInfoService> healthInfoServiceMock;
        private HealthInfosController healthInfoController;

        public TestHealthInfoController()
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

            healthInfoServiceMock = new Mock<IHealthInfoService>();

            healthInfoController = new HealthInfosController(healthInfoServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfHealthInfo()
        {
            // Arrange
            healthInfoServiceMock.Setup(service => service.GetAll()).Returns(healthInfos);

            // Act
            var result = healthInfoController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthInfo>>();
            result.Value.Should().BeEquivalentTo(healthInfos);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoHealthInfoExist()
        {
            // Arrange
            healthInfoServiceMock.Setup(service => service.GetAll()).Returns(new List<HealthInfo>());

            // Act
            var result = healthInfoController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthInfo>>();
            result.Value.As<List<HealthInfo>>().Should().HaveCount(0);
        }

        [Fact]
        public void GetById_ReturnsHealthInfoWithValidId()
        {
            // Arrange
            var healthInfoIdToFind = 1;
            var healthInfoToReturn = new HealthInfo { Id = healthInfoIdToFind, Date = new DateTime(2023, 1, 28) };
            healthInfoServiceMock.Setup(service => service.GetById(healthInfoIdToFind)).Returns(healthInfoToReturn);

            // Act
            var result = healthInfoController.GetById(healthInfoIdToFind) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<HealthInfo>();
            result.Value.Should().BeEquivalentTo(healthInfoToReturn);
        }

        [Fact]
        public void GetById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            var invalidHealthInfoId = 999;
            healthInfoServiceMock.Setup(service => service.GetById(invalidHealthInfoId)).Returns((HealthInfo)null);

            // Act
            var result = healthInfoController.GetById(invalidHealthInfoId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
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
            var result = healthInfoController.Create(newHealthInfo) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            result.ActionName.Should().Be("GetById");
            result.RouteValues["id"].Should().Be(newHealthInfo.Id);
            result.Value.Should().Be(newHealthInfo);
        }

        [Fact]
        public void Update_ReturnsOkResultForValidHealthInfo()
        {
            // Arrange
            var healthInfoIdToUpdate = 1;
            var updatedHealthInfo = new HealthInfo
            {
                Id = healthInfoIdToUpdate,
                Date = new DateTime(2023, 12, 5),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 135,
                LowerBloodPreassure = 88,
                SugarLevel = 97,
                FatPercentage = 23.0,
                Weight = 73.2,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            healthInfoServiceMock.Setup(service => service.Update(updatedHealthInfo));

            // Act
            var result = healthInfoController.Update(healthInfoIdToUpdate, updatedHealthInfo) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(updatedHealthInfo);
        }

        [Fact]
        public void Update_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidHealthInfoId = 999;
            var updatedHealthInfo = new HealthInfo
            {
                Id = invalidHealthInfoId,
                Date = new DateTime(2023, 12, 5),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 135,
                LowerBloodPreassure = 88,
                SugarLevel = 97,
                FatPercentage = 23.0,
                Weight = 73.2,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            healthInfoServiceMock.Setup(service => service.Update(updatedHealthInfo)).Throws(new Exception());

            // Act
            var result = healthInfoController.Update(invalidHealthInfoId, updatedHealthInfo) as BadRequestResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Delete_ReturnsNoContentForValidHealthInfo()
        {
            // Arrange
            var healthInfoIdToDelete = 1;
            var healthInfoToDelete = new HealthInfo
            {
                Id = healthInfoIdToDelete,
                Date = new DateTime(2023, 1, 28),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 120,
                LowerBloodPreassure = 80,
                SugarLevel = 90,
                FatPercentage = 20.5,
                Weight = 70.0,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            healthInfoServiceMock.Setup(service => service.GetById(healthInfoIdToDelete)).Returns(healthInfoToDelete);

            // Act
            var result = healthInfoController.Delete(healthInfoIdToDelete) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
            healthInfoServiceMock.Verify(service => service.Delete(healthInfoToDelete), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFoundForInvalidHealthInfo()
        {
            // Arrange
            var invalidHealthInfoId = 999;
            healthInfoServiceMock.Setup(service => service.GetById(invalidHealthInfoId)).Returns((HealthInfo)null);

            // Act
            var result = healthInfoController.Delete(invalidHealthInfoId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void GetAllOfOwner_ReturnsHealthInfosForMatchingOwner()
        {
            // Arrange
            var targetOwnersJmbg = 1234567890;
            var healthInfosWithMatchingJmbg = healthInfos.Where(h => h.OwnersJmbg == targetOwnersJmbg).ToList();
            healthInfoServiceMock.Setup(service => service.GetAllOfOwner(targetOwnersJmbg)).Returns(healthInfosWithMatchingJmbg);

            // Act
            var result = healthInfoController.GetAllOfOwner(targetOwnersJmbg) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthInfo>>();
            result.Value.Should().BeEquivalentTo(healthInfosWithMatchingJmbg);
        }

        [Fact]
        public void GetAllOfOwner_ReturnsNotFoundForNoMatchingHealthInfos()
        {
            // Arrange
            var targetOwnersJmbg = 999999999; // Use a JMBG that doesn't exist in your test data
            healthInfoServiceMock.Setup(service => service.GetAllOfOwner(targetOwnersJmbg)).Returns(new List<HealthInfo>());

            // Act
            var result = healthInfoController.GetAllOfOwner(targetOwnersJmbg) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

    }
}
