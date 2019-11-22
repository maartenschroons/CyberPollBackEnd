using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollBackend.Models;
using PollBackend.Services;

namespace PollBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PollContext _context;
        private IUserService _userService;
        public UserController(PollContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        //Authenticate user
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //Get list of users by name and email
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersLikeName(string name)
        {
            var userList = await _context.Users.ToListAsync();
            var users = new List<User>();
            //Check every user if the name or email contains the given value and add it to the list
            foreach (var user in userList)
            {
                if (user.Username.Contains(name) || user.Email.Contains(name))
                {
                    users.Add(user);
                }
            }
            return users;
        }

        //Get a list of users by email only
        [HttpGet("email/{email}")]
        public async Task<ActionResult<User>> GetUsersByEmail(string email)
        {
            var userList = await _context.Users.ToListAsync();
            //Check every user if the email contains the given value and add it to the list
            foreach (var user in userList)
            {
                if (user.Email.Contains(email))
                {
                    return user;
                }
            }
            return null;
        }


        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User     
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
