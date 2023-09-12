using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationsController : ControllerBase
    {
        private readonly IInformationService _informationService;

        public InformationsController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        // GET: api/informations
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_informationService.GetAll());
        }

        // GET api/informations/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var information = _informationService.GetById(id);
            if (information == null)
            {
                return NotFound();
            }

            return Ok(information);
        }

        // POST api/informations
        [HttpPost]
        public ActionResult Create(Information information)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _informationService.Create(information);
            return CreatedAtAction("GetById", new { id = information.Id }, information);
        }

        // PUT api/informations/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, Information information)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != information.Id)
            {
                return BadRequest();
            }

            try
            {
                _informationService.Update(information);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(information);
        }

        // DELETE api/informations/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var information = _informationService.GetById(id);
            if (information == null)
            {
                return NotFound();
            }

            _informationService.Delete(information);
            return NoContent();
        }

        // GET api/informations/accepted
        [HttpGet("accepted")]
        public ActionResult GetAllAccepted()
        {
            var acceptedInformation = _informationService.GetAllAccepted();
            return Ok(acceptedInformation);
        }

        // GET api/informations/waiting
        [HttpGet("waiting")]
        public ActionResult GetAllWaiting()
        {
            var waitingInformation = _informationService.GetAllWaiting();
            return Ok(waitingInformation);
        }

        // POST api/informations/accept/2
        [HttpPost("accept/{id}")]
        public ActionResult AcceptInformation(int id)
        {
            var information = _informationService.GetById(id);
            if (information == null)
            {
                return NotFound(new { message = "Information not found." });
            }

            try
            {
                _informationService.AcceptInformation(information);
                return Ok("Information accepted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST api/informations/decline/2
        [HttpPost("decline/{id}")]
        public ActionResult DeclineInformation(int id)
        {
            var information = _informationService.GetById(id);
            if (information == null)
            {
                return NotFound(new { message = "Information not found." });
            }

            try
            {
                _informationService.DeclineInformation(information);
                return Ok(new { message = "Information declined successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
