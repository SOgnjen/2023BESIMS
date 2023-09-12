using HospitalLibrary.Core.Model;
using HospitalLibrary.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public class InformationRepository : IInformationRepository
    {
        private HospitalDbContext _context;

        public InformationRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public void AcceptInformation(Information information)
        {
            var existingInformation = _context.informations.Find(information.Id);

            if (existingInformation != null && existingInformation.Status == InformationStatus.Waiting)
            {
                existingInformation.Status = InformationStatus.Accepted;
                _context.SaveChanges();
            }
        }

        public void Create(Information information)
        {
            _context.informations.Add(information);
            _context.SaveChanges();
        }

        public void DeclineInformation(Information information)
        {
            var existingInformation = _context.informations.Find(information.Id);

            if (existingInformation != null && existingInformation.Status == InformationStatus.Waiting)
            {
                existingInformation.Status = InformationStatus.Declined;
                _context.SaveChanges();
            }
        }

        public void Delete(Information information)
        {
            _context.informations.Remove(information);
            _context.SaveChanges();
        }

        public IEnumerable<Information> GetAll()
        {
            return _context.informations.ToList();
        }

        public IEnumerable<Information> GetAllAccepted()
        {
            return _context.informations.Where(info => info.Status == InformationStatus.Accepted).ToList();
        }

        public IEnumerable<Information> GetAllWaiting()
        {
            return _context.informations.Where(info => info.Status == InformationStatus.Waiting).ToList();
        }

        public Information GetById(int id)
        {
            return _context.informations.Find(id);
        }

        public void Update(Information information)
        {
            _context.Entry(information).State = EntityState.Modified;

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
