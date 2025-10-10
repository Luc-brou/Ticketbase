using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<IActionResult> Create(Concert concert)
        {
            if (ModelState.IsValid)
            {
                concert.CreateDate = DateTime.Now;

                if (concert.ConcertPhoto != null && concert.ConcertPhoto.Length > 0)
                {
                    var photosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
                    if (!Directory.Exists(photosPath))
                        Directory.CreateDirectory(photosPath);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(concert.ConcertPhoto.FileName);
                    var filePath = Path.Combine(photosPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await concert.ConcertPhoto.CopyToAsync(stream);

                    // Save filename to DB
                    concert.Filename = fileName;
                }

                try
                {
                    _context.Add(concert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine("DB Update Error: " + ex.InnerException?.Message);
                    throw;
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title", concert.GenreID);
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
        public async Task<IActionResult> Edit(int id, Concert concert)
        {
            if (id != concert.ConcertID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle new photo upload
                    if (concert.ConcertPhoto != null && concert.ConcertPhoto.Length > 0)
                    {
                        var photosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
                        if (!Directory.Exists(photosPath))
                            Directory.CreateDirectory(photosPath);

                        // Delete old file if it exists
                        if (!string.IsNullOrEmpty(concert.Filename))
                        {
                            var oldPath = Path.Combine(photosPath, concert.Filename);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }

                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(concert.ConcertPhoto.FileName);
                        var filePath = Path.Combine(photosPath, fileName);

                        using var stream = new FileStream(filePath, FileMode.Create);
                        await concert.ConcertPhoto.CopyToAsync(stream);

                        // ✅ Update the filename in the entity
                        concert.Filename = fileName;
                    }

                    _context.Update(concert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Concerts.Any(e => e.ConcertID == concert.ConcertID))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title", concert.GenreID);
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
                // Delete associated photo file
                if (!string.IsNullOrEmpty(concert.Filename))
                {
                    var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", concert.Filename);
                    if (System.IO.File.Exists(photoPath))
                    {
                        System.IO.File.Delete(photoPath);
                    }
                }

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