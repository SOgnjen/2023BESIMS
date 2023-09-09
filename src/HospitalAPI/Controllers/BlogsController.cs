using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: api/blogs
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_blogService.GetAll());
        }

        // GET api/blogs/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var blog = _blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // POST api/blogs
        [HttpPost]
        public ActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _blogService.Create(blog);
            return CreatedAtAction("GetById", new { id = blog.Id }, blog);
        }

        // PUT api/blogs/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.Id)
            {
                return BadRequest();
            }

            try
            {
                _blogService.Update(blog);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(blog);
        }

        // DELETE api/blogs/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var blog = _blogService.GetById(id);
            if (blog == null)
            {
                return NotFound();
            }

            _blogService.Delete(blog);
            return NoContent();
        }
    }
}
