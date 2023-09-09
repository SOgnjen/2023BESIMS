using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class HealthReviewService : IHealthReviewService
    {
        private readonly IHealthReviewRepository _healthReviewRepository;

        public HealthReviewService(IHealthReviewRepository healthReviewRepository)
        {
            _healthReviewRepository = healthReviewRepository;
        }

        public void Create(HealthReview healthReview)
        {
            _healthReviewRepository.Create(healthReview);
        }

        public void Delete(HealthReview healthReview)
        {
            _healthReviewRepository.Delete(healthReview);
        }

        public IEnumerable<HealthReview> GetAll()
        {
            return _healthReviewRepository.GetAll();
        }

        public HealthReview GetById(int id)
        {
            return _healthReviewRepository.GetById(id);
        }

        public void Update(HealthReview healthReview)
        {
            _healthReviewRepository.Update(healthReview);
        }
    }
}
