using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ticketbase.Data;
using Ticketbase.Models;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
    private readonly TicketbaseContext _context;
    public PurchasesController(TicketbaseContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        => await _context.Purchase.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Purchase>> GetPurchase(int id)
    {
        var purchase = await _context.Purchase.FindAsync(id);
        if (purchase == null) return NotFound();
        return purchase;
    }

    [HttpPost]
    public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
    {
        _context.Purchase.Add(purchase);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPurchase), new { id = purchase.ConcertID }, purchase);
    }
}