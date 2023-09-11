using FluentAssertions;
using HospitalAPI.Controllers;
using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
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
    public class TestUserController
    {
        private readonly List<User> users;
        private Mock<IUserService> userServiceMock;
        private UsersController usersController;
        private Mock<HttpContext> httpContextMock;
        private Mock<IHttpContextAccessor> httpContextAccessorMock;

        public TestUserController()
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

            userServiceMock = new Mock<IUserService>();
            httpContextMock = new Mock<HttpContext>();

            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var emailServiceMock = new Mock<EmailService>();
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            usersController = new UsersController(userServiceMock.Object, httpContextAccessorMock.Object, emailServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfUsers()
        {
            // Arrange
            userServiceMock.Setup(service => service.GetAll()).Returns(users);

            // Act
            var result = usersController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<User>>();
            result.Value.Should().BeEquivalentTo(users);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoUsersExist()
        {
            // Arrange
            userServiceMock.Setup(service => service.GetAll()).Returns(new List<User>());

            // Act
            var result = usersController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<User>>();
            result.Value.As<List<User>>().Should().HaveCount(0);
        }

        [Fact]
        public void GetById_ReturnsUserWithValidId()
        {
            // Arrange
            var userIdToFind = 1;
            var userToReturn = new User { Id = userIdToFind, FirstName = "John" };
            userServiceMock.Setup(service => service.GetById(userIdToFind)).Returns(userToReturn);

            // Act
            var result = usersController.GetById(userIdToFind) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<User>();
            result.Value.Should().BeEquivalentTo(userToReturn);
        }

        [Fact]
        public void GetById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            var invalidUserId = 999;
            userServiceMock.Setup(service => service.GetById(invalidUserId)).Returns((User)null);

            // Act
            var result = usersController.GetById(invalidUserId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
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
            var result = usersController.Create(newUser) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            result.ActionName.Should().Be("GetById");
            result.RouteValues["id"].Should().Be(newUser.Id);
            result.Value.Should().Be(newUser);
        }

        [Fact]
        public void Update_ReturnsOkResultForValidUser()
        {
            // Arrange
            var userIdToUpdate = 1;
            var updatedUser = new User
            {
                Id = userIdToUpdate,
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

            userServiceMock.Setup(service => service.Update(updatedUser));

            // Act
            var result = usersController.Update(userIdToUpdate, updatedUser) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(updatedUser);
        }

        [Fact]
        public void Update_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidUserId = 999;
            var updatedUser = new User
            {
                Id = invalidUserId,
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

            userServiceMock.Setup(service => service.Update(updatedUser)).Throws(new Exception());

            // Act
            var result = usersController.Update(invalidUserId, updatedUser) as BadRequestResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Delete_ReturnsNoContentForValidUser()
        {
            // Arrange
            var userIdToDelete = 1;
            var userToDelete = new User
            {
                Id = userIdToDelete,
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

            userServiceMock.Setup(service => service.GetById(userIdToDelete)).Returns(userToDelete);

            // Act
            var result = usersController.Delete(userIdToDelete) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
            userServiceMock.Verify(service => service.Delete(userToDelete), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFoundForInvalidUser()
        {
            // Arrange
            var invalidUserId = 999;
            userServiceMock.Setup(service => service.GetById(invalidUserId)).Returns((User)null);

            // Act
            var result = usersController.Delete(invalidUserId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Login_ReturnsOkForValidLogin()
        {
            // Arrange
            var email = "john.doe@example.com";
            var password = "password";
            var userToReturn = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Emails = email,
                Password = password,
                Role = UserRole.Role_User,
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Jmbg = 1234567890,
                Gender = Gender.Male
            };

            userServiceMock.Setup(service => service.FindRequiredLoginUser(email, password)).Returns(userToReturn);

            var sessionMock = new Mock<ISession>();
            httpContextMock.SetupGet(c => c.Session).Returns(sessionMock.Object);

            // Act
            var result = usersController.Login(new UsersController.LoginRequestModel
            {
                Email = email,
                Password = password
            }) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public void Login_ReturnsUnauthorizedForBadCredentials()
        {
            // Arrange
            var email = "john.doe@example.com";
            var password = "wrongpassword"; // Invalid password
            var userToReturn = (User)null; // No user found

            // Configure IUserService to return null for invalid credentials
            userServiceMock.Setup(service => service.FindRequiredLoginUser(email, password)).Returns(userToReturn);

            // Act
            var result = usersController.Login(new UsersController.LoginRequestModel
            {
                Email = email,
                Password = password
            }) as UnauthorizedObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(401);
        }


        [Fact]
        public void Logout_ReturnsOk()
        {
            // Configure the HttpContext mock for sessions
            httpContextMock.SetupGet(c => c.Session).Returns(new Mock<ISession>().Object);

            // Act
            var result = usersController.Logout() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }



    }
}
