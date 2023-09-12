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
    public class HealthInfoRepository : IHealthInfoRepository
    {
        private readonly HospitalDbContext _context;

        public HealthInfoRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public void Create(HealthInfo healthInfo)
        {
            _context.HealthInfos.Add(healthInfo);
            _context.SaveChanges();
        }

        public void Delete(HealthInfo healthInfo)
        {
            _context.HealthInfos.Remove(healthInfo);
            _context.SaveChanges();
        }

        public IEnumerable<HealthInfo> GetAll()
        {
            return _context.HealthInfos.ToList();
        }

        public IEnumerable<HealthInfo> GetAllOfOwner(int ownersJmbg)
        {
            return _context.HealthInfos
                .Where(h => h.OwnersJmbg == ownersJmbg)
                .ToList();
        }



        public HealthInfo GetById(int id)
        {
            return _context.HealthInfos.Find(id);
        }

        public bool HadGoodPreassure(int ownersJmbg)
        {
            DateTime today = DateTime.Today;

            var healthInfos = _context.HealthInfos
                .Where(h => h.OwnersJmbg == ownersJmbg && h.Date >= today.AddDays(-2) && h.Date <= today)
                .ToList();

            if (healthInfos == null || healthInfos.Count == 0)
            {
                return true;
            }

            foreach (var healthInfo in healthInfos)
            {
                if (healthInfo.UpperBloodPreassure > 130 || healthInfo.LowerBloodPreassure < 80)
                {
                    return false;
                }
            }

            return true;
        }

        public bool HighFat(int ownersJmbg)
        {
            DateTime now = DateTime.Now;

            var healthInfos = _context.HealthInfos
                .Where(h => h.OwnersJmbg == ownersJmbg && h.Date >= now.AddMonths(-3) && h.Date <= now)
                .ToList();

            if (healthInfos == null || healthInfos.Count == 0)
            {
                return false;
            }

            foreach (var healthInfo in healthInfos)
            {
                if (healthInfo.FatPercentage > 33)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsInMenstruation(int ownersJmbg)
        {
            DateTime now = DateTime.Now;

            var healthInfos = _context.HealthInfos
                .Where(h => h.OwnersJmbg == ownersJmbg && h.Date >= now.AddMonths(-1) && h.Date <= now)
                .ToList();

            if (healthInfos == null || healthInfos.Count == 0)
            {
                return false;
            }

            var healthInfo = healthInfos.FirstOrDefault();

            var daysSinceLastMenstruation = (now - healthInfo?.LastMenstruation)?.Days;

            if (healthInfo == null || healthInfo.LastMenstruation == null)
            {
                return false;
            }

            return daysSinceLastMenstruation >= 28 && daysSinceLastMenstruation <= 35;
        }

        public void Update(HealthInfo healthInfo)
        {
            _context.Entry(healthInfo).State = EntityState.Modified;

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
