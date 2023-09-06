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
