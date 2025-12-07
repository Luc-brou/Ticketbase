using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;

namespace Ticketbase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchasesApiController : ControllerBase
    {
        private readonly TicketbaseContext _context;
        public PurchasesApiController(TicketbaseContext context) => _context = context;

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            return await _context.Purchase.ToListAsync();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            _context.Purchase.Add(purchase);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostPurchase), new { id = purchase.TicketID }, purchase);
        }
    }
}