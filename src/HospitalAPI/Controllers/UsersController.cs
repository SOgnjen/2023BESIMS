using HospitalLibrary.Core.Model;
using HospitalLibrary.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailService _emailService;

        public UsersController(IUserService userService, IHttpContextAccessor httpContextAccessor, EmailService emailService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
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

            var existingUserWithSameFirstName = _userService.GetAll().FirstOrDefault(u => u.Emails == user.Emails);
            if (existingUserWithSameFirstName != null)
            {
                return Conflict(new { Message = "A user with the same first name already exists." });
            }

            var existingUserWithSameJmbg = _userService.GetAll().FirstOrDefault(u => u.Jmbg == user.Jmbg);
            if (existingUserWithSameJmbg != null)
            {
                return Conflict(new { Message = "A user with the same JMBG already exists." });
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

            // Check if the user is blocked
            if (user.IsBlocked)
            {
                return Unauthorized(new { Message = "Your account has been blocked" });
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

        // GET api/users/{userId}/guidance-users
        [HttpGet("{userId}/guidance-users")]
        public ActionResult GetUsersByGuidanceForUser(int userId)
        {
            var user = _userService.GetById(userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var users = _userService.GetUsersBasedOnGuidance(user.Guidance);

            if (users == null || !users.Any())
            {
                return NotFound("No users found based on guidance");
            }

            return Ok(users);
        }

        // PUT api/users/update-declines/2
        [HttpPut("update-declines/{id}")]
        public ActionResult UpdateNumberOfDeclines(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.NumberOfDeclines += 1;

            try
            {
                _userService.Update(user);
            }
            catch (Exception)
            {
                return BadRequest("Failed to update numberOfDeclines");
            }

            return Ok(user);
        }

        // GET api/users/all-bad-users
        [HttpGet("all-bad-users")]
        public ActionResult GetAllBadUsers()
        {
            var badUsers = _userService.GetAllBadUsers();

            if (badUsers == null || !badUsers.Any())
            {
                return NotFound("No bad users found");
            }

            return Ok(badUsers);
        }


        [HttpPut("block-user/{id}")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.IsBlocked = !user.IsBlocked;

            string emailMessage = user.IsBlocked ? "You have been blocked." : "You have been unblocked.";
            await _emailService.SendEmailAsync(user.Emails, "Account Status Update", emailMessage);

            _userService.Update(user);

            return Ok(user);
        }
    }
}
