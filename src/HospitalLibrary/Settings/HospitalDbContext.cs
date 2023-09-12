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
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthReview> HealthReviews { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Information> informations { get; set; }
        public DbSet<BloodAppointment> blood_appointments { get; set; }


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
                    Gender = Gender.Male,
                    IsBlocked = false,
                    Guidance = GuidanceTo.Dermatologist,
                    NumberOfDeclines = 0
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
                    Gender = Gender.Female,
                    IsBlocked = false,
                    Guidance = GuidanceTo.None,
                    NumberOfDeclines = 0
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
                    Gender = Gender.Male,
                    IsBlocked = false,
                    Guidance = GuidanceTo.None,
                    NumberOfDeclines = 0
                },
                new User
                {
                    Id = 4,
                    FirstName = "Sarah",
                    LastName = "Brown",
                    Emails = "sarah.brown@example.com",
                    Password = "password",
                    Role = UserRole.Role_Dermatologist,
                    Address = "321 Oak St",
                    PhoneNumber = "555-4321",
                    Jmbg = 22222222,
                    Gender = Gender.Female,
                    IsBlocked = false,
                    Guidance = GuidanceTo.None,
                    NumberOfDeclines = 0
                },
                new User
                {
                    Id = 5,
                    FirstName = "Michael",
                    LastName = "Clark",
                    Emails = "michael.clark@example.com",
                    Password = "password",
                    Role = UserRole.Role_Neurologist,
                    Address = "567 Pine St",
                    PhoneNumber = "555-8765",
                    Jmbg = 44444444,
                    Gender = Gender.Male,
                    IsBlocked = false,
                    Guidance = GuidanceTo.None,
                    NumberOfDeclines = 0
                },
                new User
                {
                    Id = 6,
                    FirstName = "Emily",
                    LastName = "Wilson",
                    Emails = "emily.wilson@example.com",
                    Password = "password",
                    Role = UserRole.Role_Psychiatrist,
                    Address = "789 Cedar St",
                    PhoneNumber = "555-9876",
                    Jmbg = 33333333,
                    Gender = Gender.Female,
                    IsBlocked = false,
                    Guidance = GuidanceTo.None,
                    NumberOfDeclines = 0
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

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 15, 10, 0, 0)
                },
                new Appointment
                {
                    Id = 2,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 18, 14, 30, 0)
                },
                new Appointment
                {
                    Id = 3,
                    DoctorJmbg = 987654321,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 20, 9, 15, 0)
                },
                new Appointment
                {
                    Id = 4,
                    DoctorJmbg = 22222222,
                    PatientJmbg = 1234567890,
                    Date = new DateTime(2023, 10, 22, 11, 0, 0)
                },
                new Appointment
                {
                    Id = 5,
                    DoctorJmbg = 22222222,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 10, 25, 16, 45, 0)
                },
                new Appointment
                {
                    Id = 6,
                    DoctorJmbg = 44444444,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 10, 28, 8, 30, 0)
                },
                new Appointment
                {
                    Id = 7,
                    DoctorJmbg = 44444444,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 10, 30, 13, 15, 0)
                },
                new Appointment
                {
                    Id = 8,
                    DoctorJmbg = 33333333,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 11, 2, 9, 0, 0)
                },
                new Appointment
                {
                    Id = 9,
                    DoctorJmbg = 33333333,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 11, 5, 15, 30, 0)
                },
                new Appointment
                {
                    Id = 10,
                    DoctorJmbg = 22222222,
                    PatientJmbg = 0,
                    Date = new DateTime(2023, 11, 8, 10, 45, 0)
                }
            );

            modelBuilder.Entity<HealthReview>().HasData(
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
                new HealthReview
                {
                    Id = 4,
                    Date = new DateTime(2023, 10, 20),
                    PatientJmbg = 1234567890,
                    Diagnosis = "Migraine Headache",
                    Cure = "Pain relievers and rest advised"
                },
                new HealthReview
                {
                    Id = 5,
                    Date = new DateTime(2023, 10, 25),
                    PatientJmbg = 1234567890,
                    Diagnosis = "Stomachache",
                    Cure = "Dietary changes recommended"
                }
            );

            modelBuilder.Entity<Blog>().HasData(
                new Blog
                {
                    Id = 1,
                    WriterJmbg = 987654321,
                    Title = "The Importance of Regular Check-ups",
                    Text = "Regular check-ups are essential for maintaining good health. They help detect health issues early and prevent complications. Make sure to schedule your check-up today!"
                },
                new Blog
                {
                    Id = 2,
                    WriterJmbg = 22222222,
                    Title = "Tips for Healthy Skin",
                    Text = "Healthy skin is a reflection of overall well-being. In this blog, we'll share tips for maintaining healthy and radiant skin. Remember to stay hydrated and protect your skin from the sun!"
                },
                new Blog
                {
                    Id = 3,
                    WriterJmbg = 33333333,
                    Title = "Understanding Stress and Coping Strategies",
                    Text = "Stress is a common issue in today's fast-paced world. In this blog, we'll delve into the causes of stress and provide effective coping strategies. Remember, it's essential to prioritize your mental health."
                },
                new Blog
                {
                    Id = 4,
                    WriterJmbg = 44444444,
                    Title = "The Brain-Boosting Benefits of Exercise",
                    Text = "Exercise not only benefits your body but also your brain. Learn how physical activity can boost cognitive function, improve memory, and reduce the risk of neurological conditions. Get moving for a healthier brain!"
                },
                new Blog
                {
                    Id = 5,
                    WriterJmbg = 987654321,
                    Title = "Nutrition and Its Impact on Overall Health",
                    Text = "Proper nutrition is the cornerstone of good health. This blog explores the importance of a balanced diet, the role of nutrients, and how dietary choices can affect your overall well-being."
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
