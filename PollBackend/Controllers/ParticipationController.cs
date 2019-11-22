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
    public class ParticipationController : ControllerBase
    {
        private readonly PollContext _context;

        public ParticipationController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Participation

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Participation>>> GetParticipations()
        {
            return await _context.Participations.ToListAsync();
        }

        // GET: api/Participation/5

        [HttpGet("{id}")]
        public async Task<ActionResult<Participation>> GetParticipation(long id)
        {
            var participation = await _context.Participations.FindAsync(id);

            if (participation == null)
            {
                return NotFound();
            }

            return participation;
        }

        //Get a list of polls by participator
        [HttpGet("polls/{id}")]
        public async Task<ActionResult<IEnumerable<Poll>>> GetPollsByUser(long id)
        {
            var participationList = await _context.Participations.ToListAsync();
            var polls = new List<Poll>();
            //Check participation if the user is participating in it and add the poll to the list
            foreach (var participation in participationList)
            {
                if (participation.UserId == id)
                {
                    var poll = await _context.Polls.FindAsync(participation.PollId);
                    polls.Add(poll);
                }
            }

            return polls;
        }

        //Get a list of users by participation
        [HttpGet("users/{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByPoll(long id)
        {
            var participationList = await _context.Participations.ToListAsync();
            var users = new List<User>();

            //Check every participation if it is associated with the poll and add the user to the list
            foreach (var participation in participationList)
            {
                if (participation.PollId == id)
                {
                    var user = await _context.Users.FindAsync(participation.PollId);
                    users.Add(user);
                }
            }

            return users;
        }

        //Get a list of participations by pollid
        [HttpGet("part/{id}")]
        public async Task<ActionResult<IEnumerable<Participation>>> GetParticipationsByPoll(long id)
        {
            var participationList = await _context.Participations.ToListAsync();
            var participations = new List<Participation>();

            //Check participation if it is associated to the poll and add it to the list
            foreach (var participation in participationList)
            {
                if (participation.PollId == id)
                {
                    participations.Add(participation);
                }
            }

            return participations;
        }

        // PUT: api/Participation/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParticipation(long id, Participation participation)
        {
            if (id != participation.ParticipationId)
            {
                return BadRequest();
            }

            _context.Entry(participation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipationExists(id))
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

        // POST: api/Participation
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Participation>> PostParticipation(Participation participation)
        {
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipation", new { id = participation.ParticipationId }, participation);
        }

        // DELETE: api/Participation/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Participation>> DeleteParticipation(long id)
        {
            var participation = await _context.Participations.FindAsync(id);
            if (participation == null)
            {
                return NotFound();
            }

            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();

            return participation;
        }

        private bool ParticipationExists(long id)
        {
            return _context.Participations.Any(e => e.ParticipationId == id);
        }
    }
}
