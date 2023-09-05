using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthInfoController : ControllerBase
    {
        private readonly IHealthInfoService _healthInfoService;
        
        public HealthInfoController(IHealthInfoService healthInfoService)
        {
            _healthInfoService = healthInfoService;
        }

        // GET: api/healthInfos
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_healthInfoService.GetAll());
        }

        // GET api/healthInfos/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var healthInfo = _healthInfoService.GetById(id);
            if (healthInfo == null)
            {
                return NotFound();
            }

            return Ok(healthInfo);
        }

        // POST api/healthInfos
        [HttpPost]
        public ActionResult Create(HealthInfo healthInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _healthInfoService.Create(healthInfo);
            return CreatedAtAction("GetById", new { id = healthInfo.Id }, healthInfo);
        }

        // PUT api/healthInfos/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, HealthInfo healthInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != healthInfo.Id)
            {
                return BadRequest();
            }

            try
            {
                _healthInfoService.Update(healthInfo);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(healthInfo);
        }

        // DELETE api/healthInfos/2
        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            var healthInfo = _healthInfoService.GetById(id);
            if (healthInfo == null)
            {
                return NotFound();
            }

            _healthInfoService.Delete(healthInfo);
            return NoContent();
        }
    }
}
