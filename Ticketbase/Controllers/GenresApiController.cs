using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Ticketbase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresApiController : ControllerBase
    {
        private readonly TicketbaseContext _context;
        public GenresApiController(TicketbaseContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
            => await _context.Genres.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return NotFound();
            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenre), new { id = genre.GenreID }, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.GenreID) return BadRequest();
            _context.Entry(genre).State = EntityState.Modified;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Genres.Any(e => e.GenreID == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null) return NotFound();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}