using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        [StringLength(280, ErrorMessage = "Comment must be 280 characters or less")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public int PostId { get; set; }
    }

    public class UpdateCommentDto
    {
        [Required]
        [StringLength(280, ErrorMessage = "Comment must be 280 characters or less")]
        public string Content { get; set; } = string.Empty;
    }

    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? UserDisplayName { get; set; }
        public int PostId { get; set; }
    }
}
