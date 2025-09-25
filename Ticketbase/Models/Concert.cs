using System.ComponentModel.DataAnnotations;

namespace Ticketbase.Models
{
    public class Concert
    {
        [Display(Name = "Concert Id")]
        public int EventId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Filename { get; set; } = string.Empty;

        [Display(Name = "Concert Date")]
        public string EventDate { get; set; } = string.Empty;

    }
}