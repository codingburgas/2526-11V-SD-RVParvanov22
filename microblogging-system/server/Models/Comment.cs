using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        [StringLength(280, ErrorMessage = "Comment must be 280 characters or less")]
        public string Content { get; set; } = string.Empty;

        // Foreign keys
        [Required]
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Post? Post { get; set; }
    }
}
