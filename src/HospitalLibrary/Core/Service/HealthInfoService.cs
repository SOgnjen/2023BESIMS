using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class HealthInfoService : IHealthInfoService
    {
        private readonly IHealthInfoRepository _healthInfoRepository;

        public HealthInfoService(IHealthInfoRepository healthInfoRepository)
        {
            _healthInfoRepository = healthInfoRepository;
        }

        public void Create(HealthInfo healthInfo)
        {
            _healthInfoRepository.Create(healthInfo);
        }

        public void Delete(HealthInfo healthInfo)
        {
            _healthInfoRepository.Delete(healthInfo);
        }

        public IEnumerable<HealthInfo> GetAll()
        {
            return _healthInfoRepository.GetAll();
        }

        public HealthInfo GetById(int id)
        {
            return _healthInfoRepository.GetById(id);
        }

        public void Update(HealthInfo healthInfo)
        {
            _healthInfoRepository.Update(healthInfo);
        }
    }
}
