using System;
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
            if (id == null) return NotFound();

            var concert = await _context.Concerts
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(m => m.ConcertID == id);

            if (concert == null) return NotFound();

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

                    concert.Filename = fileName;
                }

                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title", concert.GenreID);
            return View(concert);
        }

        // GET: Concerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var concert = await _context.Concerts.FindAsync(id);
            if (concert == null) return NotFound();

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title", concert.GenreID);
            return View(concert);
        }

        // POST: Concerts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Concert concert)
        {
            if (id != concert.ConcertID) return NotFound();

            var existingConcert = await _context.Concerts.FindAsync(id);
            if (existingConcert == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingConcert.Title = concert.Title;
                existingConcert.Description = concert.Description;
                existingConcert.ConcertDate = concert.ConcertDate;
                existingConcert.GenreID = concert.GenreID;

                if (concert.ConcertPhoto != null && concert.ConcertPhoto.Length > 0)
                {
                    var photosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
                    if (!Directory.Exists(photosPath))
                        Directory.CreateDirectory(photosPath);

                    if (!string.IsNullOrEmpty(existingConcert.Filename))
                    {
                        var oldPath = Path.Combine(photosPath, existingConcert.Filename);
                        if (System.IO.File.Exists(oldPath))
                            System.IO.File.Delete(oldPath);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(concert.ConcertPhoto.FileName);
                    var filePath = Path.Combine(photosPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await concert.ConcertPhoto.CopyToAsync(stream);

                    existingConcert.Filename = fileName;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewData["GenreID"] = new SelectList(_context.Genres, "GenreID", "Title", concert.GenreID);
            return View(concert);
        }

        // GET: Concerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var concert = await _context.Concerts
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(m => m.ConcertID == id);

            if (concert == null) return NotFound();

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
                if (!string.IsNullOrEmpty(concert.Filename))
                {
                    var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", concert.Filename);
                    if (System.IO.File.Exists(photoPath))
                        System.IO.File.Delete(photoPath);
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