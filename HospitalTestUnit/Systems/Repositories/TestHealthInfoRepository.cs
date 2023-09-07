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
    public class TestHealthInfoRepository : IDisposable
    {
        private readonly List<HealthInfo> healthInfos;
        private HospitalDbContext dbContext;

        public TestHealthInfoRepository()
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
        public void GetAll_ReturnAllHealthInfos()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.HealthInfos.AddRange(healthInfos);
            dbContext.SaveChanges();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            // Act
            var result = healthInfoRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthInfos);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoHealthInfosExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            // Act
            var result = healthInfoRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsHealthInfoWithValidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int healthInfoToFind = 1;
            dbContext.HealthInfos.AddRange(healthInfos);
            dbContext.SaveChanges();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            // Act
            var result = healthInfoRepository.GetById(healthInfoToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(healthInfos.FirstOrDefault(h => h.Id == healthInfoToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int healthInfoToFind = 999;
            dbContext.HealthInfos.AddRange(healthInfos);
            dbContext.SaveChanges();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            // Act
            var result = healthInfoRepository.GetById(healthInfoToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddHealthInfoToDatabase()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthInfoRepository = new HealthInfoRepository(dbContext);
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
            healthInfoRepository.Create(newHealthInfo);
            dbContext.SaveChanges();

            // Assert
            var addedHealthInfo = dbContext.HealthInfos.FirstOrDefault(h => h.Id == newHealthInfo.Id);
            addedHealthInfo.Should().NotBeNull();
            addedHealthInfo.Date.Should().Be(newHealthInfo.Date);
            addedHealthInfo.OwnersJmbg.Should().Be(newHealthInfo.OwnersJmbg);
            addedHealthInfo.UpperBloodPreassure.Should().Be(newHealthInfo.UpperBloodPreassure);
            addedHealthInfo.LowerBloodPreassure.Should().Be(newHealthInfo.LowerBloodPreassure);
            addedHealthInfo.SugarLevel.Should().Be(newHealthInfo.SugarLevel);
            addedHealthInfo.FatPercentage.Should().Be(newHealthInfo.FatPercentage);
            addedHealthInfo.Weight.Should().Be(newHealthInfo.Weight);
            addedHealthInfo.LastMenstruation.Should().Be(newHealthInfo.LastMenstruation);
        }

        [Fact]
        public void Update_ModifiesExistingHealthInfo()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            var healthInfoToAdd = new HealthInfo
            {
                Date = new DateTime(2023, 12, 5),
                OwnersJmbg = 1234567890,
                UpperBloodPreassure = 135,
                LowerBloodPreassure = 88,
                SugarLevel = 97,
                FatPercentage = 23.0,
                Weight = 73.2,
                LastMenstruation = new DateTime(1900, 1, 1)
            };

            dbContext.HealthInfos.Add(healthInfoToAdd);
            dbContext.SaveChanges();

            var retrievedHealthInfo = dbContext.HealthInfos.FirstOrDefault(h => h.Id == healthInfoToAdd.Id);

            retrievedHealthInfo.Date = new DateTime(2023, 12, 5);
            retrievedHealthInfo.UpperBloodPreassure = 135;
            retrievedHealthInfo.LowerBloodPreassure = 88;
            retrievedHealthInfo.SugarLevel = 97;
            retrievedHealthInfo.FatPercentage = 23.0;
            retrievedHealthInfo.Weight = 73.2;

            // Act
            healthInfoRepository.Update(retrievedHealthInfo);
            dbContext.SaveChanges();

            var updatedHealthInfo = dbContext.HealthInfos.FirstOrDefault(h => h.Id == healthInfoToAdd.Id);

            // Assert
            updatedHealthInfo.Should().NotBeNull();
            updatedHealthInfo.Date.Should().Be(new DateTime(2023, 12, 5));
            updatedHealthInfo.UpperBloodPreassure.Should().Be(135);
            updatedHealthInfo.LowerBloodPreassure.Should().Be(88);
            updatedHealthInfo.SugarLevel.Should().Be(97);
            updatedHealthInfo.FatPercentage.Should().Be(23.0);
            updatedHealthInfo.Weight.Should().Be(73.2);
        }

        [Fact]
        public void Delete_ExistingHealthInfo_SuccessfullyDeletesHealthInfo()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthInfoRepository = new HealthInfoRepository(dbContext);

            var healthInfoToDelete = new HealthInfo
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

            dbContext.HealthInfos.Add(healthInfoToDelete);
            dbContext.SaveChanges();

            // Act
            healthInfoRepository.Delete(healthInfoToDelete);
            dbContext.SaveChanges();

            var deletedHealthInfo = dbContext.HealthInfos.FirstOrDefault(h => h.Id == healthInfoToDelete.Id);

            // Assert
            deletedHealthInfo.Should().BeNull();
        }

        [Fact]
        public void GetAllOfOwner_ReturnsHealthInfosWithMatchingOwnersJmbg()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.HealthInfos.AddRange(healthInfos);
            dbContext.SaveChanges();
            var healthInfoRepository = new HealthInfoRepository(dbContext);
            var targetOwnersJmbg = 1234567890;

            // Act
            var result = healthInfoRepository.GetAllOfOwner(targetOwnersJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(healthInfos.Where(h => h.OwnersJmbg == targetOwnersJmbg));
        }

        [Fact]
        public void GetAllOfOwner_ReturnsEmptyListWhenNoHealthInfosForOwnerExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var healthInfoRepository = new HealthInfoRepository(dbContext);
            var targetOwnersJmbg = 99999999;

            // Act
            var result = healthInfoRepository.GetAllOfOwner(targetOwnersJmbg);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }


    }
}
