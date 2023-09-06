using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }

        // GET api/users/2
        [HttpGet("{id}")]
        public ActionResult GetById(int id) 
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST api/users
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _userService.Create(user);
            return CreatedAtAction("GetById", new { id = user.Id }, user);
        }

        // PUT api/users/2
        [HttpPut("{id}")]
        public ActionResult Update(int id, User user) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                _userService.Update(user);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(user);
        }

        // DELETE api/users/2
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _userService.Delete(user);
            return NoContent();
        }

        // POST api/users/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            var user = _userService.FindRequiredLoginUser(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized(new { Message = "Bad credentials" });
            }

            var sessionId = Guid.NewGuid().ToString();

            _httpContextAccessor.HttpContext.Session.SetString("SessionId", sessionId);

            _httpContextAccessor.HttpContext.Session.SetInt32("UserId", user.Id);
            _httpContextAccessor.HttpContext.Session.SetString("UserEmail", user.Emails);
            _httpContextAccessor.HttpContext.Session.SetString("UserFirstName", user.FirstName);
            _httpContextAccessor.HttpContext.Session.SetString("UserLastName", user.LastName);
            _httpContextAccessor.HttpContext.Session.SetString("UserRole", user.Role.ToString());
            _httpContextAccessor.HttpContext.Session.SetString("UserAddress", user.Address);
            _httpContextAccessor.HttpContext.Session.SetString("UserPhoneNumber", user.PhoneNumber);
            _httpContextAccessor.HttpContext.Session.SetInt32("UserJmbg", user.Jmbg);
            _httpContextAccessor.HttpContext.Session.SetString("UserGender", user.Gender.ToString());

            return Ok(new { Message = "Login successful", User = user });
        }

        public class LoginRequestModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // POST api/users/logout
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Remove("SessionId");

            return Ok(new { Message = "Logout successful" });
        }
    }
}
