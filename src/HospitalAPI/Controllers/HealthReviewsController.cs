using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthReviewsController : ControllerBase
    {
        private readonly IHealthReviewService _healthReviewService;

        public HealthReviewsController(IHealthReviewService healthReviewService)
        {
            _healthReviewService = healthReviewService;
        }

        // GET: api/healthReviews
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_healthReviewService.GetAll());
        }

        // GET api/healthReviews/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var healthReview = _healthReviewService.GetById(id);
            if (healthReview == null)
            {
                return NotFound();
            }

            return Ok(healthReview);
        }

        // POST api/healthReviews
        [HttpPost]
        public ActionResult Create(HealthReview healthReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _healthReviewService.Create(healthReview);
            return CreatedAtAction("GetById", new { id = healthReview.Id }, healthReview);
        }

        // PUT api/healthReviews/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, HealthReview healthReview)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != healthReview.Id)
            {
                return BadRequest();
            }

            try
            {
                _healthReviewService.Update(healthReview);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(healthReview);
        }

        // DELETE api/healthReviews/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var healthReview = _healthReviewService.GetById(id);
            if (healthReview == null)
            {
                return NotFound();
            }

            _healthReviewService.Delete(healthReview);
            return NoContent();
        }
    }
}
