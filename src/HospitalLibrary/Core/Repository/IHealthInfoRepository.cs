using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public interface IHealthInfoRepository
    {
        IEnumerable<HealthInfo> GetAll();
        HealthInfo GetById(int id);
        void Create(HealthInfo healthInfo);
        void Update(HealthInfo healthInfo);
        void Delete(HealthInfo healthInfo);
        IEnumerable<HealthInfo> GetAllOfOwner(int ownersJmbg);
        bool HadGoodPreassure(int ownersJmbg);
        bool HighFat(int ownersJmbg);
        bool IsInMenstruation(int ownersJmbg);
    }
}
