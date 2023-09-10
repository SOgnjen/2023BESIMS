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
    public class TestHealthReviewController
    {
        private readonly List<HealthReview> healthReviews;
        private Mock<IHealthReviewService> healthReviewServiceMock;
        private HealthReviewsController healthReviewController;

        public TestHealthReviewController()
        {
            healthReviews = new List<HealthReview>
            {
                new HealthReview
                {
                    Id = 1,
                    Date = new DateTime(2023, 10, 5),
                    PatientJmbg = 1234567890,
                    Diagnosis = "Common Cold",
                    Cure = "Rest and fluids"
                },
                new HealthReview
                {
                    Id = 2,
                    Date = new DateTime(2023, 10, 10),
                    PatientJmbg = 1234567890,
                    Diagnosis = "Allergic Reaction",
                    Cure = "Antihistamines prescribed"
                },
                new HealthReview
                {
                    Id = 3,
                    Date = new DateTime(2023, 10, 15),
                    PatientJmbg = 1234567890,
                    Diagnosis = "Sprained Ankle",
                    Cure = "RICE protocol (Rest, Ice, Compression, Elevation)"
                },
            };

            healthReviewServiceMock = new Mock<IHealthReviewService>();

            healthReviewController = new HealthReviewsController(healthReviewServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfHealthReviews()
        {
            // Arrange
            healthReviewServiceMock.Setup(service => service.GetAll()).Returns(healthReviews);

            // Act
            var result = healthReviewController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthReview>>();
            result.Value.Should().BeEquivalentTo(healthReviews);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoHealthReviewsExist()
        {
            // Arrange
            healthReviewServiceMock.Setup(service => service.GetAll()).Returns(new List<HealthReview>());

            // Act
            var result = healthReviewController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthReview>>();
            result.Value.As<List<HealthReview>>().Should().HaveCount(0);
        }

        [Fact]
        public void GetById_ReturnsHealthReviewWithValidId()
        {
            // Arrange
            var healthReviewIdToFind = 1;
            var healthReviewToReturn = new HealthReview { Id = healthReviewIdToFind, Date = new DateTime(2023, 10, 5) };
            healthReviewServiceMock.Setup(service => service.GetById(healthReviewIdToFind)).Returns(healthReviewToReturn);

            // Act
            var result = healthReviewController.GetById(healthReviewIdToFind) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<HealthReview>();
            result.Value.Should().BeEquivalentTo(healthReviewToReturn);
        }

        [Fact]
        public void GetById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            var invalidHealthReviewId = 999;
            healthReviewServiceMock.Setup(service => service.GetById(invalidHealthReviewId)).Returns((HealthReview)null);

            // Act
            var result = healthReviewController.GetById(invalidHealthReviewId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var newHealthReview = new HealthReview
            {
                Date = new DateTime(2023, 11, 10),
                PatientJmbg = 1234567890,
                Diagnosis = "Fever",
                Cure = "Prescribed medication"
            };

            // Act
            var result = healthReviewController.Create(newHealthReview) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            result.ActionName.Should().Be("GetById");
            result.RouteValues["id"].Should().Be(newHealthReview.Id);
            result.Value.Should().Be(newHealthReview);
        }

        [Fact]
        public void Update_ReturnsOkResultForValidHealthReview()
        {
            // Arrange
            var healthReviewIdToUpdate = 1;
            var updatedHealthReview = new HealthReview
            {
                Id = healthReviewIdToUpdate,
                Date = new DateTime(2023, 12, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Headache",
                Cure = "Rest and pain relievers"
            };

            healthReviewServiceMock.Setup(service => service.Update(updatedHealthReview));

            // Act
            var result = healthReviewController.Update(healthReviewIdToUpdate, updatedHealthReview) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(updatedHealthReview);
        }

        [Fact]
        public void Update_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidHealthReviewId = 999;
            var updatedHealthReview = new HealthReview
            {
                Id = invalidHealthReviewId,
                Date = new DateTime(2023, 12, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Headache",
                Cure = "Rest and pain relievers"
            };

            healthReviewServiceMock.Setup(service => service.Update(updatedHealthReview)).Throws(new Exception());

            // Act
            var result = healthReviewController.Update(invalidHealthReviewId, updatedHealthReview) as BadRequestResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Delete_ReturnsNoContentForValidHealthReview()
        {
            // Arrange
            var healthReviewIdToDelete = 1;
            var healthReviewToDelete = new HealthReview
            {
                Id = healthReviewIdToDelete,
                Date = new DateTime(2023, 10, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Common Cold",
                Cure = "Rest and fluids"
            };

            healthReviewServiceMock.Setup(service => service.GetById(healthReviewIdToDelete)).Returns(healthReviewToDelete);

            // Act
            var result = healthReviewController.Delete(healthReviewIdToDelete) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
            healthReviewServiceMock.Verify(service => service.Delete(healthReviewToDelete), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFoundForInvalidHealthReview()
        {
            // Arrange
            var invalidHealthReviewId = 999;
            healthReviewServiceMock.Setup(service => service.GetById(invalidHealthReviewId)).Returns((HealthReview)null);

            // Act
            var result = healthReviewController.Delete(invalidHealthReviewId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void GetAllOfPatient_ReturnsHealthReviewsForMatchingPatient()
        {
            // Arrange
            var targetPatientJmbg = 1234567890;
            var healthReviewsWithMatchingJmbg = healthReviews.Where(h => h.PatientJmbg == targetPatientJmbg).ToList();
            healthReviewServiceMock.Setup(service => service.GetAllOfPatient(targetPatientJmbg)).Returns(healthReviewsWithMatchingJmbg);

            // Act
            var result = healthReviewController.GetAllOfPatient(targetPatientJmbg) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<HealthReview>>();
            result.Value.Should().BeEquivalentTo(healthReviewsWithMatchingJmbg);
        }

        [Fact]
        public void GetAllOfPatient_ReturnsNotFoundForNoMatchingHealthReviews()
        {
            // Arrange
            var targetPatientJmbg = 999999999;
            healthReviewServiceMock.Setup(service => service.GetAllOfPatient(targetPatientJmbg)).Returns(new List<HealthReview>());

            // Act
            var result = healthReviewController.GetAllOfPatient(targetPatientJmbg) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }


    }
}
