using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Models;

namespace Ticketbase.Data
{
    public class TicketbaseContext : DbContext
    {
        public TicketbaseContext(DbContextOptions<TicketbaseContext> options)
            : base(options)
        {
        }

        public DbSet<Concert> Concerts { get; set; } = default!;
        public DbSet<Genre> Genres { get; set; } = default!;
    }
}