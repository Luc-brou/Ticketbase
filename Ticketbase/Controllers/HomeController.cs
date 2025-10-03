using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;

namespace Ticketbase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TicketbaseContext _context;

        public HomeController(ILogger<HomeController> logger, TicketbaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var concerts = await _context.Concerts
                .Include(c => c.Genre) 
                .ToListAsync();

            return View(concerts);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var concert = await _context.Concerts
                .Include(c => c.Genre) 
                .FirstOrDefaultAsync(c => c.ConcertID == id);

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        public IActionResult Info()
        {
            return View();
        }

    }
}