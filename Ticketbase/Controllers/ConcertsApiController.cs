using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;

[ApiController]
[Route("api/[controller]")]
public class ConcertsController : ControllerBase
{
    private readonly TicketbaseContext _context;

    public ConcertsController(TicketbaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Concert>>> GetConcerts()
    {
        return await _context.Concerts.ToListAsync(); // <-- returns JSON automatically
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Concert>> GetConcert(int id)
    {
        var concert = await _context.Concerts.FindAsync(id);
        if (concert == null) return NotFound();
        return concert; // <-- JSON
    }
}