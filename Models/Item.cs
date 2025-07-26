using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SemesterProj.Models
{
    public class Item
    {
        [Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = string.Empty;

        public string? Image { get; set; }  // Stores file path
        [NotMapped]
        public IFormFile? ImageFile { get; set; }


        [Required(ErrorMessage = "Minimum bid is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Must be positive value")]
        public decimal MinBid { get; set; }

        public decimal CurrentBid { get; set; } = 0;

        [Required(ErrorMessage = "Bid increment is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Must be positive value")]
        public decimal BidIncrement { get; set; }

        public string BidStatus { get; set; } = "Pending"; // Pending/Active/Inactive

        [Required(ErrorMessage = "Start date is required")]
        public DateTime BidStartDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "End date is required")]
        public DateTime BidEndDate { get; set; } = DateTime.Now.AddDays(7);

        [Required]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Categories? Category { get; set; } // Navigation property
        // Foreign Key to Auth
        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        public Auth? Seller { get; set; } // Navigation property
    }

}