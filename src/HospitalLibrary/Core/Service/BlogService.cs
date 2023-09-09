using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalLibrary.Core.Service
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public void Create(Blog blog)
        {
            _blogRepository.Create(blog);
        }

        public void Delete(Blog blog)
        {
            _blogRepository.Delete(blog);
        }

        public IEnumerable<Blog> GetAll()
        {
            return _blogRepository.GetAll();
        }

        public Blog GetById(int id)
        {
            return _blogRepository.GetById(id);
        }

        public void Update(Blog blog)
        {
            _blogRepository.Update(blog);
        }
    }
}
