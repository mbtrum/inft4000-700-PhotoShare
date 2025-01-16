namespace PhotoShare.Models
{
    public class Tag
    {
        // Primary key
        public int TagId { get; set; }

        public string Name { get; set; } = string.Empty;

        // Foreign key
        public int PhotoId { get; set; }

        // Navigation property
        public Photo? Photo { get; set; } // nullable!
    }

}
