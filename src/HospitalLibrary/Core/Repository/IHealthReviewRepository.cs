using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public interface IHealthReviewRepository
    {
        IEnumerable<HealthReview> GetAll();
        HealthReview GetById(int id);
        void Create(HealthReview healthReview);
        void Update(HealthReview healthReview);
        void Delete(HealthReview healthReview);
    }
}
