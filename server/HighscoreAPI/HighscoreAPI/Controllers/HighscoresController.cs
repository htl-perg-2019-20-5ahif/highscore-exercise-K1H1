using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HighscoreAPI;
using HighscoreAPI.Model;

namespace HighscoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighscoresController : ControllerBase
    {
        private readonly DataContext _context;

        public HighscoresController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Highscores
        [HttpGet]
        public IEnumerable<Highscore> GetHighscores()
        {

			//order the highscorelist
			return  _context.HighScores.OrderByDescending(h => h.Score).ToList();
		}

        // GET: api/Highscores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Highscore>> GetHighscore(Guid id)
        {
            var highscore = await _context.HighScores.FindAsync(id);

            if (highscore == null)
            {
                return NotFound();
            }

            return highscore;
        }

 

        // POST: api/Highscores
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Highscore>> PostHighscore(Highscore highscore)
        {

			// if list is full, check if the score is bigger than one already in the highscore list
			// add directly if list has less than 10 entries
			if (_context.HighScores.Count() >= 10)
			{
				var lastHighscore = _context.HighScores.OrderByDescending(h => h.Score).Last();
				if (lastHighscore.Score < highscore.Score)
				{
					_context.HighScores.Remove(lastHighscore);
				}
				else
				{
					return BadRequest("This score is not a highscore");
				}
			}
			
			_context.HighScores.Add(highscore);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetHighScore", new { id = highscore.HighScoreId }, highscore);
		}


		// DELETE: api/Highscores/5
		[HttpDelete("{id}")]
        public async Task<ActionResult<Highscore>> DeleteHighscore(Guid id)
        {
            var highscore = await _context.HighScores.FindAsync(id);
            if (highscore == null)
            {
                return NotFound();
            }

            _context.HighScores.Remove(highscore);
            await _context.SaveChangesAsync();

            return highscore;
        }

        private bool HighscoreExists(Guid id)
        {
            return _context.HighScores.Any(e => e.HighScoreId == id);
        }


		

	}
}
