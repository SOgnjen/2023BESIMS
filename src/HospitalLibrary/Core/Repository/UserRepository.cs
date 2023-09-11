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
    public class UserRepository : IUserRepository
    {
        private readonly HospitalDbContext _context;

        public UserRepository(HospitalDbContext context)
        {
            _context = context;
        }

        public void BlockUser(User user)
        {
            user.IsBlocked = !user.IsBlocked; // Toggle the IsBlocked property

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User FindUserByEmailAndPassword(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Emails == email && u.Password == password);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public IEnumerable<User> GetAllBadUsers()
        {
            return _context.Users.Where(u => u.NumberOfDeclines >= 3).ToList();
        }

        public IEnumerable<User> GetAllUsersWithSameRole(UserRole role)
        {
            return _context.Users.Where(u => u.Role == role).ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByJmbg(int jmbg)
        {
            return _context.Users.FirstOrDefault(u => u.Jmbg == jmbg);
        }

        public IEnumerable<User> GetUsersBasedOnGuidance(GuidanceTo guidance)
        {
            IQueryable<User> query = _context.Users;

            switch (guidance)
            {
                case GuidanceTo.Dermatologist:
                    query = query.Where(u => u.Role == UserRole.Role_Medic || u.Role == UserRole.Role_Dermatologist);
                    break;
                case GuidanceTo.Neurologist:
                    query = query.Where(u => u.Role == UserRole.Role_Medic || u.Role == UserRole.Role_Neurologist);
                    break;
                case GuidanceTo.Psychiatrist:
                    query = query.Where(u => u.Role == UserRole.Role_Medic || u.Role == UserRole.Role_Psychiatrist);
                    break;
                default:
                    break;
            }

            return query.ToList();
        }

        public void Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

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
