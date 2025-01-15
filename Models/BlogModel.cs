using System.ComponentModel.DataAnnotations;
namespace iBlog.Models
{
    public class BlogModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = String.Empty;
        [Required]
        public string Content { get; set; } = String.Empty;
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        [Required]
        public string Category { get; set; } = String.Empty;
        public string? ImageUrlPath { get; set; }
        public string AuthorId { get; set; } = String.Empty; //Author Id
        public virtual ApplicationUser Author { get; set; } = new(); //Navigation property


    }
}
