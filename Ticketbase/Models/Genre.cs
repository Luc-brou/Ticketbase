using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Ticketbase.Models
{
    public class Genre
    {
        // Primary key
        [Display(Name = "Genre Id")]

        public int GenreID { get; set; }

        public string Title { get; set; } = string.Empty;

        // Navigation Property (code)
        public List<Concert>? Concerts { get; set; }

    }
}
