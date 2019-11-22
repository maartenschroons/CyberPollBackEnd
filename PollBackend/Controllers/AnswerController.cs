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
    public class AnswerController : ControllerBase
    {
        private readonly PollContext _context;

        public AnswerController(PollContext context)
        {
            _context = context;
        }

        // GET: api/Answer

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            return await _context.Answers.ToListAsync();
        }


        //Get a list of answers by userId
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswersByUser(long id)
        {
            var answerList = await _context.Answers.ToListAsync();
            var answers = new List<Answer>();

            //Check the answer if the userId equals the given id and add it to the list
            foreach (var answer in answerList)
            {
                if (answer.UserId == id)
                {
                    answers.Add(answer);
                }
            }
            return answers;
        }

        //Get a list of answers by pollId
        [HttpGet("poll/{id}")]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswersByPoll(long id)
        {
            var answerList = await _context.Answers.ToListAsync();
            var answers = new List<Answer>();
            var voteList = await _context.Votes.ToListAsync();

            //Check the answer if the pollId equals the given id and add it to the list
            foreach (var answer in answerList)
            {
                var votes = new List<Vote>();
                if (answer.PollId == id)
                {
                    //Check each vote if it belongs to the answer and add it to the list
                    foreach (var vote in voteList)
                    {
                        if (vote.AnswerId == answer.AnswerId)
                        {
                            votes.Add(vote);
                        }
                    }
                    //Add the amount of votes to the answer
                    answer.voteAmount = votes.Count();
                    answers.Add(answer);
                }
            }
            return answers;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(long id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        // PUT: api/Answer/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(long id, Answer answer)
        {
            if (id != answer.AnswerId)
            {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // POST: api/Answer
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.AnswerId }, answer);
        }

        // DELETE: api/Answer/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Answer>> DeleteAnswer(long id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return answer;
        }

        private bool AnswerExists(long id)
        {
            return _context.Answers.Any(e => e.AnswerId == id);
        }
    }
}
