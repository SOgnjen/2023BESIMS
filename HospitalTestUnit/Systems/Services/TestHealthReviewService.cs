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
    public class TestHealthReviewService
    {
        private readonly List<HealthReview> healthReviews;
        private Mock<IHealthReviewRepository> healthReviewRepositoryMock;
        private HealthReviewService healthReviewService;

        public TestHealthReviewService()
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

            healthReviewRepositoryMock = new Mock<IHealthReviewRepository>();
            healthReviewService = new HealthReviewService(healthReviewRepositoryMock.Object);
        }

        [Fact]
        public void GetAllHealthReviews_ReturnAllHealthReviews()
        {
            // Arrange
            healthReviewRepositoryMock.Setup(repo => repo.GetAll()).Returns(healthReviews);

            // Act
            var result = healthReviewService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthReviews);
        }

        [Fact]
        public void GetAllHealthReviews_ReturnsEmptyListWhenNoHealthReviewsExist()
        {
            // Arrange
            healthReviewRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<HealthReview>());

            // Act
            var result = healthReviewService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetHealthReviewById_ReturnsHealthReviewWithValidId()
        {
            // Arrange
            var healthReviewIdToFind = 1;
            var healthReviewToReturn = new HealthReview { Id = healthReviewIdToFind, Date = new DateTime(2023, 10, 5) };
            healthReviewRepositoryMock.Setup(repo => repo.GetById(healthReviewIdToFind)).Returns(healthReviewToReturn);

            // Act
            var result = healthReviewService.GetById(healthReviewIdToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthReviewToReturn);
        }

        [Fact]
        public void GetHealthReviewById_ReturnsNullForInvalidId()
        {
            // Arrange
            var invalidHealthReviewId = 999;
            healthReviewRepositoryMock.Setup(repo => repo.GetById(invalidHealthReviewId)).Returns((HealthReview)null);

            // Act
            var result = healthReviewService.GetById(invalidHealthReviewId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void CreateHealthReview_AddsHealthReviewToRepository()
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
            healthReviewService.Create(newHealthReview);

            // Assert
            healthReviewRepositoryMock.Verify(repo => repo.Create(newHealthReview), Times.Once);
        }

        [Fact]
        public void UpdateHealthReview_UpdatesHealthReviewInRepository()
        {
            // Arrange
            var healthReviewToUpdate = new HealthReview
            {
                Id = 1,
                Date = new DateTime(2023, 12, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Headache",
                Cure = "Rest and pain relievers"
            };

            // Act
            healthReviewService.Update(healthReviewToUpdate);

            // Assert
            healthReviewRepositoryMock.Verify(repo => repo.Update(healthReviewToUpdate), Times.Once);
        }

        [Fact]
        public void DeleteHealthReview_DeletesHealthReviewFromRepository()
        {
            // Arrange
            var healthReviewToDelete = new HealthReview
            {
                Id = 1,
                Date = new DateTime(2023, 10, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Common Cold",
                Cure = "Rest and fluids"
            };

            // Act
            healthReviewService.Delete(healthReviewToDelete);

            // Assert
            healthReviewRepositoryMock.Verify(repo => repo.Delete(healthReviewToDelete), Times.Once);
        }
    }
}
