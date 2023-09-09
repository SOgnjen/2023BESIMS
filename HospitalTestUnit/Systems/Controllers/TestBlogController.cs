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
    public class TestBlogController
    {
        private readonly List<Blog> blogs;
        private Mock<IBlogService> blogServiceMock;
        private BlogsController blogController;

        public TestBlogController()
        {
            blogs = new List<Blog>
            {
                new Blog
                {
                    Id = 1,
                    WriterJmbg = 987654321, // Medic
                    Title = "The Importance of Regular Check-ups",
                    Text = "Regular check-ups are essential for maintaining good health. They help detect health issues early and prevent complications. Make sure to schedule your check-up today!"
                },
                new Blog
                {
                    Id = 2,
                    WriterJmbg = 22222222, // Dermatologist
                    Title = "Tips for Healthy Skin",
                    Text = "Healthy skin is a reflection of overall well-being. In this blog, we'll share tips for maintaining healthy and radiant skin. Remember to stay hydrated and protect your skin from the sun!"
                },
                new Blog
                {
                    Id = 3,
                    WriterJmbg = 33333333, // Psychiatrist
                    Title = "Understanding Stress and Coping Strategies",
                    Text = "Stress is a common issue in today's fast-paced world. In this blog, we'll delve into the causes of stress and provide effective coping strategies. Remember, it's essential to prioritize your mental health."
                }
            };

            blogServiceMock = new Mock<IBlogService>();
            blogController = new BlogsController(blogServiceMock.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfBlogs()
        {
            // Arrange
            blogServiceMock.Setup(service => service.GetAll()).Returns(blogs);

            // Act
            var result = blogController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Blog>>();
            result.Value.Should().BeEquivalentTo(blogs);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoBlogsExist()
        {
            // Arrange
            blogServiceMock.Setup(service => service.GetAll()).Returns(new List<Blog>());

            // Act
            var result = blogController.GetAll() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<List<Blog>>();
            result.Value.As<List<Blog>>().Should().HaveCount(0);
        }

        [Fact]
        public void GetById_ReturnsBlogWithValidId()
        {
            // Arrange
            var blogIdToFind = 1;
            var blogToReturn = new Blog { Id = blogIdToFind, WriterJmbg = 987654321 };
            blogServiceMock.Setup(service => service.GetById(blogIdToFind)).Returns(blogToReturn);

            // Act
            var result = blogController.GetById(blogIdToFind) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().BeOfType<Blog>();
            result.Value.Should().BeEquivalentTo(blogToReturn);
        }

        [Fact]
        public void GetById_ReturnsNotFoundForInvalidId()
        {
            // Arrange
            var invalidBlogId = 999;
            blogServiceMock.Setup(service => service.GetById(invalidBlogId)).Returns((Blog)null);

            // Act
            var result = blogController.GetById(invalidBlogId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var newBlog = new Blog
            {
                WriterJmbg = 44444444, // Neurologist
                Title = "The Benefits of Quality Sleep",
                Text = "Quality sleep is essential for brain health and overall well-being. In this blog, we'll explore the importance of sleep and provide tips for better sleep quality."
            };

            // Act
            var result = blogController.Create(newBlog) as CreatedAtActionResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(201);
            result.ActionName.Should().Be("GetById");
            result.RouteValues["id"].Should().Be(newBlog.Id);
            result.Value.Should().Be(newBlog);
        }

        [Fact]
        public void Update_ReturnsOkResultForValidBlog()
        {
            // Arrange
            var blogIdToUpdate = 1;
            var updatedBlog = new Blog
            {
                Id = blogIdToUpdate,
                WriterJmbg = 987654321, // Medic
                Title = "Updated Blog Title",
                Text = "Updated Blog Text"
            };

            blogServiceMock.Setup(service => service.Update(updatedBlog));

            // Act
            var result = blogController.Update(blogIdToUpdate, updatedBlog) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            result.Value.Should().Be(updatedBlog);
        }

        [Fact]
        public void Update_ReturnsBadRequestForInvalidId()
        {
            // Arrange
            var invalidBlogId = 999;
            var updatedBlog = new Blog
            {
                Id = invalidBlogId,
                WriterJmbg = 987654321, // Medic
                Title = "Updated Blog Title",
                Text = "Updated Blog Text"
            };

            blogServiceMock.Setup(service => service.Update(updatedBlog)).Throws(new Exception());

            // Act
            var result = blogController.Update(invalidBlogId, updatedBlog) as BadRequestResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(400);
        }

        [Fact]
        public void Delete_ReturnsNoContentForValidBlog()
        {
            // Arrange
            var blogIdToDelete = 1;
            var blogToDelete = new Blog
            {
                Id = blogIdToDelete,
                WriterJmbg = 987654321, // Medic
                Title = "The Importance of Regular Check-ups",
                Text = "Regular check-ups are essential for maintaining good health. They help detect health issues early and prevent complications. Make sure to schedule your check-up today!"
            };

            blogServiceMock.Setup(service => service.GetById(blogIdToDelete)).Returns(blogToDelete);

            // Act
            var result = blogController.Delete(blogIdToDelete) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(204);
            blogServiceMock.Verify(service => service.Delete(blogToDelete), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFoundForInvalidBlog()
        {
            // Arrange
            var invalidBlogId = 999;
            blogServiceMock.Setup(service => service.GetById(invalidBlogId)).Returns((Blog)null);

            // Act
            var result = blogController.Delete(invalidBlogId) as NotFoundResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(404);
        }
    }
}
