using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollBackend.Models;

namespace PollBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly PollContext _context;

        public FriendshipController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Friendship

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendships()
        {
            return await _context.Friendships.ToListAsync();
        }

        // GET: api/Friendship/5

        [HttpGet("{id}")]
        public async Task<ActionResult<Friendship>> GetFriendship(long id)
        {
            var friendship = await _context.Friendships.FindAsync(id);

            if (friendship == null)
            {
                return NotFound();
            }

            return friendship;
        }

        //Get a list of friends by userId
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetFriendsByUser(long id)
        {
            var friendshipList = await _context.Friendships.ToListAsync();
            var friends = new List<User>();
            foreach (var friendship in friendshipList)
            {
                //If the user is friendA, friendB will be added to the list
                if (friendship.UserIdA == id)
                {
                    var friend = await _context.Users.FindAsync(friendship.UserIdB);
                    friends.Add(friend);
                }
                //If the user is friendB, friendA will be added to the list
                else if (friendship.UserIdB == id)
                {
                    var friend = await _context.Users.FindAsync(friendship.UserIdA);
                    friends.Add(friend);
                }
            }

            return friends;
        }

        // PUT: api/Friendship/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendship(long id, Friendship friendship)
        {
            if (id != friendship.FriendshipId)
            {
                return BadRequest();
            }

            _context.Entry(friendship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipExists(id))
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

        // POST: api/Friendship
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Friendship>> PostFriendship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriendship", new { id = friendship.FriendshipId }, friendship);
        }

        // DELETE: api/Friendship/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(long id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return friendship;
        }

        private bool FriendshipExists(long id)
        {
            return _context.Friendships.Any(e => e.FriendshipId == id);
        }
    }
}
