using System.ComponentModel.DataAnnotations;

namespace PhotoShare.Models
{
    public class Photo
    {
        // primary key
        public int PhotoId { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Camera { get; set; } = string.Empty;

        public string ImageFilename { get; set; } = string.Empty;

        [Display(Name ="Visible")]
        public bool IsVisible { get; set; } = false;

        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public List<Tag>? Tags { get; set; }  // nullable!!!
    }
}
