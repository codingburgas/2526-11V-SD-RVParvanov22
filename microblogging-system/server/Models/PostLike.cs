using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models
{
    public class PostLike : BaseEntity
    {
        // Foreign keys
        [Required]
        public string UserId { get; set; } = string.Empty;
        public int PostId { get; set; }

        // Navigation properties
        public ApplicationUser? User { get; set; }
        public Post? Post { get; set; }
    }
}
