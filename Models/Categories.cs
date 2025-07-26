namespace SemesterProj.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public required string Name { get; set; } // Ensures this property must be set
                                                  // Navigation property for related items
        //public ICollection<Item>? Items { get; set; }

    }

}
