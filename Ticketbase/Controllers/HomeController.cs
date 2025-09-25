using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ticketbase.Models;

namespace Ticketbase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Concert> photos = new List<Concert>();

            Concert Event1 = new Concert
            {
                EventId = 1,
                Title = "Radiohead North America Tour 2025",
                Description = "Radiohead is coming to scotiabank theatre.",
                Filename = "radiohead-logo.png",
                EventDate = "November 12th, 2025"
            };

            photos.Add(Event1);

            return View(photos);
        }

        public IActionResult Details(int id)
        {
            Concert Event1 = new Concert { EventId = id };

            if (Event1.EventId == 1)
            {
                Event1.Title = "Radiohead North America Tour 2025";
                Event1.Description = "Radiohead is coming to scotiabank theatre. ";
                Event1.Filename = "radiohead-logo.png";
                Event1.EventDate = "November 12th, 2025";
            }

            return View(Event1);
        }
    }
}