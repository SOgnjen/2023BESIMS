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
    public class TestBlogService
    {
        private readonly List<Blog> blogs;
        private Mock<IBlogRepository> blogRepositoryMock;
        private BlogService blogService;

        public TestBlogService()
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

            blogRepositoryMock = new Mock<IBlogRepository>();
            blogService = new BlogService(blogRepositoryMock.Object);
        }

        [Fact]
        public void GetAllBlogs_ReturnAllBlogs()
        {
            // Arrange
            blogRepositoryMock.Setup(repo => repo.GetAll()).Returns(blogs);

            // Act
            var result = blogService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(blogs);
        }

        [Fact]
        public void GetAllBlogs_ReturnsEmptyListWhenNoBlogsExist()
        {
            // Arrange
            blogRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Blog>());

            // Act
            var result = blogService.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetBlogById_ReturnsBlogWithValidId()
        {
            // Arrange
            var blogIdToFind = 1;
            var blogToReturn = new Blog { Id = blogIdToFind, WriterJmbg = 987654321 };
            blogRepositoryMock.Setup(repo => repo.GetById(blogIdToFind)).Returns(blogToReturn);

            // Act
            var result = blogService.GetById(blogIdToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(blogToReturn);
        }

        [Fact]
        public void GetBlogById_ReturnsNullForInvalidId()
        {
            // Arrange
            var invalidBlogId = 999;
            blogRepositoryMock.Setup(repo => repo.GetById(invalidBlogId)).Returns((Blog)null);

            // Act
            var result = blogService.GetById(invalidBlogId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void CreateBlog_AddsBlogToRepository()
        {
            // Arrange
            var newBlog = new Blog
            {
                WriterJmbg = 44444444, // Neurologist
                Title = "The Benefits of Quality Sleep",
                Text = "Quality sleep is essential for brain health and overall well-being. In this blog, we'll explore the importance of sleep and provide tips for better sleep quality."
            };

            // Act
            blogService.Create(newBlog);

            // Assert
            blogRepositoryMock.Verify(repo => repo.Create(newBlog), Times.Once);
        }

        [Fact]
        public void UpdateBlog_UpdatesBlogInRepository()
        {
            // Arrange
            var blogToUpdate = new Blog
            {
                Id = 1,
                WriterJmbg = 987654321, // Medic
                Title = "Updated Blog Title",
                Text = "Updated Blog Text"
            };

            // Act
            blogService.Update(blogToUpdate);

            // Assert
            blogRepositoryMock.Verify(repo => repo.Update(blogToUpdate), Times.Once);
        }

        [Fact]
        public void DeleteBlog_DeletesBlogFromRepository()
        {
            // Arrange
            var blogToDelete = new Blog
            {
                Id = 1,
                WriterJmbg = 987654321, // Medic
                Title = "The Importance of Regular Check-ups",
                Text = "Regular check-ups are essential for maintaining good health. They help detect health issues early and prevent complications. Make sure to schedule your check-up today!"
            };

            // Act
            blogService.Delete(blogToDelete);

            // Assert
            blogRepositoryMock.Verify(repo => repo.Delete(blogToDelete), Times.Once);
        }
    }
}
