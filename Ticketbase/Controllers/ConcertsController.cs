using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;

namespace Ticketbase.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly TicketbaseContext _context;

        public ConcertsController(TicketbaseContext context)
        {
            _context = context;
        }

        // GET: Concerts
        public async Task<IActionResult> Index()
        {
            var ticketbaseContext = _context.Concerts.Include(c => c.Genre);
            return View(await ticketbaseContext.ToListAsync());
        }

        // GET: Concerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var concert = await _context.Concerts
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(m => m.ConcertID == id);

            if (concert == null)
                return NotFound();

            return View(concert);
        }

        // GET: Concerts/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title");
            return View();
        }

        // POST: Concerts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertID,Title,Description,Filename,ConcertDate,GenreID")] Concert concert)
        {
            if (ModelState.IsValid)
            {
                concert.CreateDate = DateTime.Now; // Automatically set
                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", concert.GenreID);
            return View(concert);
        }

        // GET: Concerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null)
                return NotFound();

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", concert.GenreID);
            return View(concert);
        }

        // POST: Concerts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertID,Title,Description,Filename,CreateDate,ConcertDate,GenreID")] Concert concert)
        {
            if (id != concert.ConcertID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertExists(concert.ConcertID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "GenreID", concert.GenreID);
            return View(concert);
        }

        // GET: Concerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var concert = await _context.Concerts
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(m => m.ConcertID == id);

            if (concert == null)
                return NotFound();

            return View(concert);
        }

        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concert = await _context.Concerts.FindAsync(id);
            if (concert != null)
            {
                _context.Concerts.Remove(concert);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        private bool ConcertExists(int id)
        {
            return _context.Concerts.Any(e => e.ConcertID == id);
        }
    }
}