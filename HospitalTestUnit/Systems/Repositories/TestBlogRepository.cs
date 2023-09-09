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
    public class TestBlogRepository : IDisposable
    {
        private readonly List<Blog> blogs;
        private HospitalDbContext dbContext;

        public TestBlogRepository()
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
        public void GetAll_ReturnAllBlogs()
        {
            // Arrange
            dbContext = CreateDbContext();
            dbContext.Blogs.AddRange(blogs);
            dbContext.SaveChanges();
            var blogRepository = new BlogRepository(dbContext);

            // Act
            var result = blogRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(blogs);
        }

        [Fact]
        public void GetAll_ReturnsEmptyListWhenNoBlogsExist()
        {
            // Arrange
            dbContext = CreateDbContext();
            var blogRepository = new BlogRepository(dbContext);

            // Act
            var result = blogRepository.GetAll();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetById_ReturnsBlogWithValidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int blogToFind = 1;
            dbContext.Blogs.AddRange(blogs);
            dbContext.SaveChanges();
            var blogRepository = new BlogRepository(dbContext);

            // Act
            var result = blogRepository.GetById(blogToFind);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(blogs.FirstOrDefault(b => b.Id == blogToFind));
        }

        [Fact]
        public void GetById_ReturnsNullForInvalidId()
        {
            // Arrange
            dbContext = CreateDbContext();
            int blogToFind = 999;
            dbContext.Blogs.AddRange(blogs);
            dbContext.SaveChanges();
            var blogRepository = new BlogRepository(dbContext);

            // Act
            var result = blogRepository.GetById(blogToFind);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Create_AddBlogToDatabase()
        {
            // Arrange
            dbContext = CreateDbContext();
            var blogRepository = new BlogRepository(dbContext);
            var newBlog = new Blog
            {
                WriterJmbg = 44444444, // Neurologist
                Title = "The Benefits of Quality Sleep",
                Text = "Quality sleep is essential for brain health and overall well-being. In this blog, we'll explore the importance of sleep and provide tips for better sleep quality."
            };

            // Act
            blogRepository.Create(newBlog);
            dbContext.SaveChanges();

            // Assert
            var addedBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == newBlog.Id);
            addedBlog.Should().NotBeNull();
            addedBlog.WriterJmbg.Should().Be(newBlog.WriterJmbg);
            addedBlog.Title.Should().Be(newBlog.Title);
            addedBlog.Text.Should().Be(newBlog.Text);
        }

        [Fact]
        public void Update_ModifiesExistingBlog()
        {
            // Arrange
            dbContext = CreateDbContext();
            var blogRepository = new BlogRepository(dbContext);

            var blogToAdd = new Blog
            {
                WriterJmbg = 55555555, // Another Medic
                Title = "Nutrition and Its Impact on Overall Health",
                Text = "Proper nutrition is the cornerstone of good health. This blog explores the importance of a balanced diet, the role of nutrients, and how dietary choices can affect your overall well-being."
            };

            dbContext.Blogs.Add(blogToAdd);
            dbContext.SaveChanges();

            var retrievedBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == blogToAdd.Id);

            retrievedBlog.WriterJmbg = 33333333; // Psychiatrist
            retrievedBlog.Title = "Mental Health Matters";
            retrievedBlog.Text = "Taking care of your mental health is crucial for a happy life. In this blog, we'll discuss the importance of mental well-being and share tips for managing stress.";

            // Act
            blogRepository.Update(retrievedBlog);
            dbContext.SaveChanges();

            var updatedBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == blogToAdd.Id);

            // Assert
            updatedBlog.Should().NotBeNull();
            updatedBlog.WriterJmbg.Should().Be(33333333);
            updatedBlog.Title.Should().Be("Mental Health Matters");
            updatedBlog.Text.Should().Be("Taking care of your mental health is crucial for a happy life. In this blog, we'll discuss the importance of mental well-being and share tips for managing stress.");
        }

        [Fact]
        public void Delete_ExistingBlog_SuccessfullyDeletesBlog()
        {
            // Arrange
            dbContext = CreateDbContext();
            var blogRepository = new BlogRepository(dbContext);

            var blogToDelete = new Blog
            {
                WriterJmbg = 66666666, // Another User (You can replace this JMBG with an existing one)
                Title = "Living a Healthy Lifestyle",
                Text = "A healthy lifestyle is the key to a long and fulfilling life. In this blog, we'll explore the habits and practices that contribute to a healthy and active lifestyle."
            };

            dbContext.Blogs.Add(blogToDelete);
            dbContext.SaveChanges();

            // Act
            blogRepository.Delete(blogToDelete);
            dbContext.SaveChanges();

            var deletedBlog = dbContext.Blogs.FirstOrDefault(b => b.Id == blogToDelete.Id);

            // Assert
            deletedBlog.Should().BeNull();
        }
    }
}
