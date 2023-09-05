using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public interface IHealthInfoService
    {
        IEnumerable<HealthInfo> GetAll();
        HealthInfo GetById(int id);
        void Create(HealthInfo healthInfo);
        void Update(HealthInfo healthInfo);
        void Delete(HealthInfo healthInfo);
    }
}
