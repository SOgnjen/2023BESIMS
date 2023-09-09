using FluentAssertions;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Repositories
{
    public class TestHealthReviewRepository : IDisposable
    {
        private readonly List<HealthReview> healthReviews;
        private HospitalDbContext dbContext;

        public TestHealthReviewRepository()
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
        }

        private HospitalDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<HospitalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new HospitalDbContext(dbContextOptions);
        }

        public void Dispose()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }
        }

        [Fact]
        public void GetAll_ReturnAllHealthReviews()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.HealthReviews.AddRange(healthReviews);
            dbContext.SaveChanges();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            // Act
            var result = healthReviewRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthReviews);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoHealthReviewsExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            // Act
            var result = healthReviewRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsHealthReviewWithValidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int healthReviewToFind = 1;
            dbContext.HealthReviews.AddRange(healthReviews);
            dbContext.SaveChanges();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            // Act
            var result = healthReviewRepository.GetById(healthReviewToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthReviews.FirstOrDefault(h => h.Id == healthReviewToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int healthReviewToFind = 999;
            dbContext.HealthReviews.AddRange(healthReviews);
            dbContext.SaveChanges();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            // Act
            var result = healthReviewRepository.GetById(healthReviewToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddHealthReviewToDatabase()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthReviewRepository = new HealthReviewRepository(dbContext);
            var newHealthReview = new HealthReview
            {
                Date = new DateTime(2023, 11, 10),
                PatientJmbg = 1234567890,
                Diagnosis = "Fever",
                Cure = "Prescribed medication"
            };

            // Act
            healthReviewRepository.Create(newHealthReview);
            dbContext.SaveChanges();

            // Assert
            var addedHealthReview = dbContext.HealthReviews.FirstOrDefault(h => h.Id == newHealthReview.Id);
            addedHealthReview.Should().NotBeNull();
            addedHealthReview.Date.Should().Be(newHealthReview.Date);
            addedHealthReview.PatientJmbg.Should().Be(newHealthReview.PatientJmbg);
            addedHealthReview.Diagnosis.Should().Be(newHealthReview.Diagnosis);
            addedHealthReview.Cure.Should().Be(newHealthReview.Cure);
        }

        [Fact]
        public void Update_ModifiesExistingHealthReview()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            var healthReviewToAdd = new HealthReview
            {
                Date = new DateTime(2023, 12, 5),
                PatientJmbg = 1234567890,
                Diagnosis = "Headache",
                Cure = "Rest and pain relievers"
            };

            dbContext.HealthReviews.Add(healthReviewToAdd);
            dbContext.SaveChanges();

            var retrievedHealthReview = dbContext.HealthReviews.FirstOrDefault(h => h.Id == healthReviewToAdd.Id);

            retrievedHealthReview.Date = new DateTime(2023, 12, 6);
            retrievedHealthReview.Diagnosis = "Migraine";
            retrievedHealthReview.Cure = "Prescribed medication";

            // Act
            healthReviewRepository.Update(retrievedHealthReview);
            dbContext.SaveChanges();

            var updatedHealthReview = dbContext.HealthReviews.FirstOrDefault(h => h.Id == healthReviewToAdd.Id);

            // Assert
            updatedHealthReview.Should().NotBeNull();
            updatedHealthReview.Date.Should().Be(new DateTime(2023, 12, 6));
            updatedHealthReview.Diagnosis.Should().Be("Migraine");
            updatedHealthReview.Cure.Should().Be("Prescribed medication");
        }

        [Fact]
        public void Delete_ExistingHealthReview_SuccessfullyDeletesHealthReview()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthReviewRepository = new HealthReviewRepository(dbContext);

            var healthReviewToDelete = new HealthReview
            {
                Date = new DateTime(2023, 11, 10),
                PatientJmbg = 1234567890,
                Diagnosis = "Fever",
                Cure = "Prescribed medication"
            };

            dbContext.HealthReviews.Add(healthReviewToDelete);
            dbContext.SaveChanges();

            // Act
            healthReviewRepository.Delete(healthReviewToDelete);
            dbContext.SaveChanges();

            var deletedHealthReview = dbContext.HealthReviews.FirstOrDefault(h => h.Id == healthReviewToDelete.Id);

            // Assert
            deletedHealthReview.Should().BeNull();
        }
    }
}
