using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models.DTOs
{
    public class CreateFollowDto
    {
        [Required]
        public string FollowerId { get; set; } = string.Empty;

        [Required]
        public string FollowingId { get; set; } = string.Empty;
    }

    public class FollowResponseDto
    {
        public int Id { get; set; }
        public string FollowerId { get; set; } = string.Empty;
        public string FollowingId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
