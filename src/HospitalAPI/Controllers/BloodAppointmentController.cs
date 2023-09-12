using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodAppointmentController : ControllerBase
    {
        private readonly IBloodAppointmentService _bloodAppointmentService;
        private readonly IHealthInfoService _healthInfoService;

        public BloodAppointmentController(IBloodAppointmentService bloodAppointmentService, IHealthInfoService healthInfoService)
        {
            _bloodAppointmentService = bloodAppointmentService;
            _healthInfoService = healthInfoService;
        }

        // GET: api/bloodAppointments
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_bloodAppointmentService.GetAll());
        }

        // GET api/bloodAppointments/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var bloodAppointment = _bloodAppointmentService.GetById(id);
            if (bloodAppointment == null)
            {
                return NotFound();
            }

            return Ok(bloodAppointment);
        }

        // POST api/bloodAppointments
        [HttpPost]
        public ActionResult Create(BloodAppointment bloodAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bloodAppointmentService.Create(bloodAppointment);
            return CreatedAtAction("GetById", new { id = bloodAppointment.Id }, bloodAppointment);
        }

        // PUT api/bloodAppointments/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, BloodAppointment bloodAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bloodAppointment.Id)
            {
                return BadRequest();
            }

            try
            {
                _bloodAppointmentService.Update(bloodAppointment);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(bloodAppointment);
        }

        // DELETE api/bloodAppointments/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var information = _bloodAppointmentService.GetById(id);
            if (information == null)
            {
                return NotFound();
            }

            _bloodAppointmentService.Delete(information);
            return NoContent();
        }

        // GET: api/bloodAppointments/free
        [HttpGet("free")]
        public ActionResult<IEnumerable<BloodAppointment>> GetAllFree()
        {
            var freeAppointments = _bloodAppointmentService.GetAllFree();
            return Ok(freeAppointments);
        }

        // POST api/bloodAppointments/reserve
        [HttpPost("reserve")]
        public IActionResult ReserveBloodAppointment([FromBody] BloodAppointmentRequestDto requestDto)
        {
            if (!_healthInfoService.HadGoodPreassure(requestDto.UserJmbg))
            {
                return BadRequest(new { message = "The patient does not have good blood pressure." });
            }

            if (_healthInfoService.HighFat(requestDto.UserJmbg))
            {
                return BadRequest(new { message = "The patient has a high fat percentage." });
            }

            if (_healthInfoService.IsInMenstruation(requestDto.UserJmbg))
            {
                return BadRequest(new { message = "The patient is in menstruation." });
            }

            requestDto.BloodAppointment.OwnerJmbg = requestDto.UserJmbg;

            requestDto.BloodAppointment.IsFree = false;

            _bloodAppointmentService.Update(requestDto.BloodAppointment);

            return Ok(new { message = "Blood appointment successfully reserved." });
        }

        public class BloodAppointmentRequestDto
        {
            public BloodAppointment BloodAppointment { get; set; }
            public int UserJmbg { get; set; }
        }
    }
}
