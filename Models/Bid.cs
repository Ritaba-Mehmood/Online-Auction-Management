using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SemesterProj.Models
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }

        [Required]
        public int ItemId { get; set; } // Foreign Key for Item

        [ForeignKey("ItemId")]
        public Item? Item { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign Key for User

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BidAmount { get; set; }

        [Required]
        public DateTime BidTime { get; set; }
    }
}
