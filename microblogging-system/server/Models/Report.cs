using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models
{
    public class Report : BaseEntity
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string ReporterId { get; set; } = string.Empty;

        [Required]
        public ReportReason Reason { get; set; }

        public string? Description { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public ReportAction ActionTaken { get; set; } = ReportAction.None;

        // Navigation properties
        public Post? Post { get; set; }
        public ApplicationUser? Reporter { get; set; }
        public ApplicationUser? Reviewer { get; set; }
    }

    public enum ReportReason
    {
        Spam,
        Harassment,
        InappropriateContent,
        HateSpeech,
        CopyrightViolation,
        Other
    }

    public enum ReportStatus
    {
        Pending,
        Reviewed,
        Resolved
    }

    public enum ReportAction
    {
        None,
        Hidden,
        Deleted,
        WarningSent
    }
}