using System;
using System.ComponentModel.DataAnnotations;

namespace Ticketbase.Models
{
    public class Concert
    {
        [Display(Name = "Concert Id")]
        public int ConcertID { get; set; } // Primary key

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Display(Name = "File Name")]
        public string Filename { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreateDate { get; set; } // Automatically set creation date

        [Display(Name = "Concert Date")]
        public DateTime ConcertDate { get; set; } // Manually entered by user

        public int GenreID { get; set; } // Foreign key
        public Genre? Genre { get; set; } // Navigation property
    }
}