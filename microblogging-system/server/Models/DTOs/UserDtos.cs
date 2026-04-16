using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Region { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
    }

    public class UpdateUserProfileDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Display name must be between 2 and 50 characters")]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Bio must be 250 characters or less")]
        public string? Bio { get; set; }

        [Url(ErrorMessage = "Profile picture must be a valid URL")]
        public string? ProfilePictureUrl { get; set; }

        public string? Region { get; set; }
    }

    public class UserSearchResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; set; }
        public string? Region { get; set; }
        public int FollowersCount { get; set; }
        public bool IsFollowing { get; set; }
    }
}
