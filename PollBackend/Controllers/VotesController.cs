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
    public class VotesController : ControllerBase
    {
        private readonly PollContext _context;

        public VotesController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Votes

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
            return await _context.Votes.ToListAsync();
        }

        //Get a list of votes by Answerid
        [HttpGet("answer/{id}")]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotesByAnswer(long id)
        {
            var voteList = await _context.Votes.ToListAsync();
            var votes = new List<Vote>();
            //Check for each vote if it belongs to the answer and add it to the list
            foreach (var vote in voteList)
            {
                if (vote.AnswerId == id)
                {
                    votes.Add(vote);
                }
            }
            return votes;
        }

        //Get a list of votes by userId
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotesByUser(long id)
        {
            var voteList = await _context.Votes.ToListAsync();
            var votes = new List<Vote>();
            //Check for each vote if it belongs to the user and add it to the list
            foreach (var vote in voteList)
            {
                if (vote.UserId == id)
                {
                    votes.Add(vote);
                }
            }
            return votes;
        }

        // GET: api/Votes/5

        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(long id)
        {
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // PUT: api/Votes/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(long id, Vote vote)
        {
            if (id != vote.VoteId)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVote", new { id = vote.VoteId }, vote);
        }

        // DELETE: api/Votes/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vote>> DeleteVote(long id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return vote;
        }

        private bool VoteExists(long id)
        {
            return _context.Votes.Any(e => e.VoteId == id);
        }
    }
}
