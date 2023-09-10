using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmenService _appointmentService;

        public AppointmentsController(IAppointmenService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/appointments
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_appointmentService.GetAll());
        }

        // GET api/appointments/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return Ok(appointment);
        }

        // POST api/appointments
        [HttpPost]
        public ActionResult Create(Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _appointmentService.Create(appointment);
            return CreatedAtAction("GetById", new { id = appointment.Id }, appointment);
        }

        // PUT api/appointments/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointment.Id)
            {
                return BadRequest();
            }

            try
            {
                _appointmentService.Update(appointment);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(appointment);
        }

        // DELETE api/appointments/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _appointmentService.Delete(appointment);
            return NoContent();
        }

        // POST api/appointments/find
        [HttpPost("find")]
        public ActionResult FindAppointment([FromBody] FindAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var appointment = _appointmentService.FindMeAppointment(request.DoctorJmbg, request.Date, request.Priority);
                if (appointment == null)
                {
                    return NotFound("Appointment not found.");
                }

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class FindAppointmentRequest
        {
            public int DoctorJmbg { get; set; }
            [DataType(DataType.Date)]
            public DateTime Date { get; set; }
            public int Priority { get; set; }
        }


        // POST api/appointments/reserve/2
        [HttpPost("reserve/{id}")]
        public ActionResult ReserveAppointment(int id, [FromBody] ReserveAppointmentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            try
            {
                _appointmentService.ReserveAppointment(appointment, request.PatientJmbg);
                return Ok("Appointment reserved successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/appointments/decline/2
        [HttpPost("decline/{id}")]
        public ActionResult DeclineAppointment(int id)
        {
            var appointment = _appointmentService.GetById(id);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            try
            {
                _appointmentService.DeclineAppointment(appointment);
                return Ok("Appointment declined successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class ReserveAppointmentRequest
        {
            public int PatientJmbg { get; set; }
        }

        // GET api/appointments/patientsJmbg/1234567890
        [HttpGet("/patientsJmbg/{patientsJmbg}")]
        public ActionResult GetAllOfPatient(int patientsJmbg)
        {
            var appointmentsForPatient = _appointmentService.GetAllOfPatient(patientsJmbg);
            if (appointmentsForPatient == null || appointmentsForPatient.Count() == 0)
            {
                return NotFound();
            }

            return Ok(appointmentsForPatient);
        }

    }
}
