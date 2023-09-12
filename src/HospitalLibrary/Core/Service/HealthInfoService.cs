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

        public IEnumerable<HealthInfo> GetAllOfOwner(int ownersJmbg)
        {
            return _healthInfoRepository.GetAllOfOwner(ownersJmbg);
        }

        public HealthInfo GetById(int id)
        {
            return _healthInfoRepository.GetById(id);
        }

        public bool HadGoodPreassure(int ownersJmbg)
        {
            return _healthInfoRepository.HadGoodPreassure(ownersJmbg);
        }

        public bool HighFat(int ownersJmbg)
        {
            return _healthInfoRepository.HighFat(ownersJmbg);
        }

        public bool IsInMenstruation(int ownersJmbg)
        {
            return _healthInfoRepository.IsInMenstruation(ownersJmbg);
        }

        public void Update(HealthInfo healthInfo)
        {
            _healthInfoRepository.Update(healthInfo);
        }
    }
}
