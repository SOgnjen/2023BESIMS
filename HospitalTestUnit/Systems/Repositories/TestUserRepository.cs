using FluentAssertions;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using HospitalLibrary.Settings;
using HospitalTestUnit.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HospitalTestUnit.Systems.Repositories
{
    public class TestUserRepository : IDisposable
    {
        private readonly List<User> users;
        private HospitalDbContext dbContext;

        public TestUserRepository()
        {
            users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Emails = "john.doe@example.com",
                    Password = "password",
                    Role = UserRole.Role_User,
                    Address = "123 Main St",
                    PhoneNumber = "555-1234",
                    Jmbg = 1234567890,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Emails = "jane.smith@example.com",
                    Password = "password",
                    Role = UserRole.Role_Medic,
                    Address = "456 Elm St",
                    PhoneNumber = "555-5678",
                    Jmbg = 987654321,
                    Gender = Gender.Female
                },
                new User
                {
                    Id = 3,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Emails = "bob.johnson@example.com",
                    Password = "password",
                    Role = UserRole.Role_Administrator,
                    Address = "789 Oak St",
                    PhoneNumber = "555-9012",
                    Jmbg = 11111111,
                    Gender = Gender.Male
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
        public void GetAll_ReturnAllUsers()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoUsersExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsUserWithValidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int userIdToFind = 1;
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetById(userIdToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users.FirstOrDefault(u => u.Id == userIdToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int invalidUserId = 999;
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();
            var userRepository = new UserRepository(dbContext);

            // Act
            var result = userRepository.GetById(invalidUserId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddsUserToDatabase()
        {
            // Arrange
            dbContext = CreateDbContext();
            var userRepository = new UserRepository(dbContext);

            var newUser = new User
            {
                FirstName = "New",
                LastName = "User",
                Emails = "new.user@example.com",
                Password = "newpassword",
                Role = UserRole.Role_User,
                Address = "123 Elm St",
                PhoneNumber = "555-4321",
                Jmbg = 11111111,
                Gender = Gender.Female
            };

            // Act
            userRepository.Create(newUser);
            dbContext.SaveChanges();

            // Assert
            var addedUser = dbContext.Users.FirstOrDefault(u => u.Id == newUser.Id);
            addedUser.Should().NotBeNull();
            addedUser.FirstName.Should().Be(newUser.FirstName);
            addedUser.LastName.Should().Be(newUser.LastName);
            addedUser.Emails.Should().Be(newUser.Emails);
            addedUser.Role.Should().Be(newUser.Role);
            addedUser.Address.Should().Be(newUser.Address);
            addedUser.PhoneNumber.Should().Be(newUser.PhoneNumber);
            addedUser.Jmbg.Should().Be(newUser.Jmbg);
            addedUser.Gender.Should().Be(newUser.Gender);
        }

        [Fact]
        public void Update_ExistingUser_SuccessfullyUpdatesUser()
        {
            // Arrange
            dbContext = CreateDbContext();
            var userRepository = new UserRepository(dbContext);

            var userToAdd = new User
            {
                FirstName = "New",
                LastName = "User",
                Emails = "new.user@example.com",
                Password = "newpassword",
                Role = UserRole.Role_User,
                Address = "123 Elm St",
                PhoneNumber = "555-4321",
                Jmbg = 11111111,
                Gender = Gender.Female
            };

            dbContext.Users.Add(userToAdd);
            dbContext.SaveChanges();

            var retrievedUser = dbContext.Users.FirstOrDefault(u => u.Id == userToAdd.Id);

            retrievedUser.FirstName = "UpdatedFirstName";
            retrievedUser.LastName = "UpdatedLastName";
            retrievedUser.Address = "UpdatedAddress";

            // Act
            userRepository.Update(retrievedUser);
            dbContext.SaveChanges();

            var updatedUser = dbContext.Users.FirstOrDefault(u => u.Id == userToAdd.Id);

            // Assert
            updatedUser.Should().NotBeNull();
            updatedUser.FirstName.Should().Be("UpdatedFirstName");
            updatedUser.LastName.Should().Be("UpdatedLastName");
            updatedUser.Address.Should().Be("UpdatedAddress");
        }

        [Fact]
        public void Delete_ExistingUser_SuccessfullyDeletesUser()
        {
            // Arrange
            dbContext = CreateDbContext();
            var userRepository = new UserRepository(dbContext);

            var userToDelete = new User
            {
                FirstName = "User",
                LastName = "ToDelete",
                Emails = "user.to.delete@example.com",
                Password = "password",
                Role = UserRole.Role_User,
                Address = "123 Elm St",
                PhoneNumber = "555-4321",
                Jmbg = 11111111,
                Gender = Gender.Female
            };

            dbContext.Users.Add(userToDelete);
            dbContext.SaveChanges();

            // Act
            userRepository.Delete(userToDelete);
            dbContext.SaveChanges();

            var deletedUser = dbContext.Users.FirstOrDefault(u => u.Id == userToDelete.Id);

            // Assert
            deletedUser.Should().BeNull();
        }


    }
}
