using HospitalLibrary.Core.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace HospitalLibrary.Settings
{
    public class HospitalDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<HealthInfo> HealthInfos { get; set; }

        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(
                new Room() { Id = 1, Number = "101A", Floor = 1 },
                new Room() { Id = 2, Number = "204", Floor = 2 },
                new Room() { Id = 3, Number = "305B", Floor = 3 }
            );

            modelBuilder.Entity<User>().HasData(
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
            );

            modelBuilder.Entity<HealthInfo>().HasData(
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
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
