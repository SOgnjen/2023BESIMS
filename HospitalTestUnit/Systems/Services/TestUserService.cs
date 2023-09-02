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
    public class TestUserService
    {
        private readonly List<User> users;
        private Mock<IUserRepository> userRepositoryMock;
        private UserService userService;

        public TestUserService()
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

            userRepositoryMock = new Mock<IUserRepository>();
            userService = new UserService(userRepositoryMock.Object);
        }

        [Fact]
        public void GetAllUsers_ReturnAllUsers()
        {
            // Arrange
            userRepositoryMock.Setup(repo => repo.GetAll()).Returns(users);

            // Act
            var result = userService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetAllUsers_ReturnsEmptyListWhenNoUsersExist()
        {
            // Arrange
            userRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<User>());

            // Act
            var result = userService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetUserById_ReturnsUserWithValidId()
        {
            // Arrange
            var userIdToFind = 1;
            var userToReturn = new User { Id = userIdToFind, FirstName = "John" };
            userRepositoryMock.Setup(repo => repo.GetById(userIdToFind)).Returns(userToReturn);

            // Act
            var result = userService.GetById(userIdToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userToReturn);
        }

        [Fact]
        public void GetUserById_ReturnsNullForInvalidId()
        {
            // Arrange
            var invalidUserId = 999;
            userRepositoryMock.Setup(repo => repo.GetById(invalidUserId)).Returns((User)null);

            // Act
            var result = userService.GetById(invalidUserId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void CreateUser_AddsUserToRepository()
        {
            // Arrange
            var newUser = new User
            {
                FirstName = "New",
                LastName = "User",
                Emails = "new.user@example.com",
                Password = "password",
                Role = UserRole.Role_User,
                Address = "123 Elm St",
                PhoneNumber = "555-4321",
                Jmbg = 11111111,
                Gender = Gender.Female
            };

            // Act
            userService.Create(newUser);

            // Assert
            userRepositoryMock.Verify(repo => repo.Create(newUser), Times.Once);
        }

        [Fact]
        public void UpdateUser_UpdatesUserInRepository()
        {
            // Arrange
            var userToUpdate = new User
            {
                Id = 1,
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Emails = "updated.user@example.com",
                Password = "updatedpassword",
                Role = UserRole.Role_Medic,
                Address = "UpdatedAddress",
                PhoneNumber = "555-5432",
                Jmbg = 99999999,
                Gender = Gender.Male
            };

            // Act
            userService.Update(userToUpdate);

            // Assert
            userRepositoryMock.Verify(repo => repo.Update(userToUpdate), Times.Once);
        }

        [Fact]
        public void DeleteUser_DeletesUserFromRepository()
        {
            // Arrange
            var userToDelete = new User
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
            };

            // Act
            userService.Delete(userToDelete);

            // Assert
            userRepositoryMock.Verify(repo => repo.Delete(userToDelete), Times.Once);
        }
    }
}
