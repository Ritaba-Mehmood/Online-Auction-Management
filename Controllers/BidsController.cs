using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SemesterProj.Data;
using SemesterProj.Models;

namespace SemesterProj.Controllers
{
    public class BidsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BidsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult PlaceBid(int ItemId, decimal BidAmount)
        {
            var item = _context.Items.FirstOrDefault(i => i.ItemId == ItemId);

            if (item == null)
            {
                return NotFound();
            }

            // Check if bid is valid
            if (BidAmount <= item.CurrentBid)
            {
                TempData["Error"] = "Your bid must be higher than the current bid!";
                return RedirectToAction("Details", "Items", new { id = ItemId });
            }

            // Save bid to the database
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "0"); // Replace with your UserID fetching logic

            var bid = new Bid
            {
                ItemId = ItemId,
                UserId = userId,
                BidAmount = BidAmount,
                BidTime = DateTime.Now
            };

            _context.Bids.Add(bid);
            item.CurrentBid = BidAmount; // Update current bid for the item
            _context.SaveChanges();

            TempData["Success"] = "Your bid has been placed successfully!";
            return RedirectToAction("Details", "Items", new { id = ItemId });
        }
    }
}
