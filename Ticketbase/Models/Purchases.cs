using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketbase.Models
{
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TicketID { get; set; }               // Primary key

        [Required]
        public int ConcertID { get; set; }               // FK to Concerts table

        [Required]
        public int NumTicketsOrdered { get; set; }

        [Required]
        [MaxLength(500)]
        public string CustomerDetails { get; set; } = null!;

        [MaxLength(200)]
        public string? CardToken { get; set; }

    }
}