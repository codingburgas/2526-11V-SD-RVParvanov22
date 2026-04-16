using System.ComponentModel.DataAnnotations;

namespace MicrobloggingSystem.Models.DTOs
{
    public class CreateReportDto
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public Models.ReportReason Reason { get; set; }

        [StringLength(500, ErrorMessage = "Description must be 500 characters or less")]
        public string? Description { get; set; }
    }

    public class ReportResponseDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string ReporterId { get; set; } = string.Empty;
        public string ReporterDisplayName { get; set; } = string.Empty;
        public Models.ReportReason Reason { get; set; }
        public string? Description { get; set; }
        public Models.ReportStatus Status { get; set; }
        public string? ReviewedBy { get; set; }
        public string? ReviewerDisplayName { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Models.ReportAction ActionTaken { get; set; }
        public DateTime CreatedAt { get; set; }

        // Post information for context
        public string PostContent { get; set; } = string.Empty;
        public string PostAuthorDisplayName { get; set; } = string.Empty;
        public string? PostGameTitle { get; set; }
    }

    public class UpdateReportDto
    {
        [Required]
        public Models.ReportStatus Status { get; set; }

        [Required]
        public Models.ReportAction ActionTaken { get; set; }
    }
}