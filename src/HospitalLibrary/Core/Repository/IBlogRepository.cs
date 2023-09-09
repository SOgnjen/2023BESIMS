using HospitalLibrary.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Repository
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> GetAll();
        Blog GetById(int id);
        void Create(Blog blog);
        void Update(Blog blog);
        void Delete(Blog blog);
    }
}
