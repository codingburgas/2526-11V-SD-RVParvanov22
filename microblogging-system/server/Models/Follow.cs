using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models
{
    public class Follow : BaseEntity
    {
        // Foreign keys
        [Required]
        public string FollowerId { get; set; } = string.Empty;

        [Required]
        public string FollowingId { get; set; } = string.Empty;

        // Navigation properties
        public ApplicationUser? Follower { get; set; }
        public ApplicationUser? Following { get; set; }
    }
}
