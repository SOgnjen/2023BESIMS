using HospitalLibrary.Core.Model;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public class HealthReviewRepository : IHealthReviewRepository
    {
        private HospitalDbContext _context;

        public HealthReviewRepository(HospitalDbContext dbContext)
        {
            this._context = dbContext;
        }

        public void Create(HealthReview healthReview)
        {
            _context.HealthReviews.Add(healthReview);
            _context.SaveChanges();
        }

        public void Delete(HealthReview healthReview)
        {
            _context.HealthReviews.Remove(healthReview);
            _context.SaveChanges();
        }

        public IEnumerable<HealthReview> GetAll()
        {
            return _context.HealthReviews.ToList();
        }

        public IEnumerable<HealthReview> GetAllOfPatient(int patientsJmbg)
        {
            return _context.HealthReviews
                .Where(h => h.PatientJmbg == patientsJmbg)
                .ToList();
        }

        public HealthReview GetById(int id)
        {
            return _context.HealthReviews.Find(id);
        }

        public void Update(HealthReview healthReview)
        {
            _context.Entry(healthReview).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}
