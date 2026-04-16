using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReportResponseDto> CreateReportAsync(CreateReportDto createReportDto, string reporterId)
        {
            // Check if post exists
            var post = await _context.Posts.FindAsync(createReportDto.PostId);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }

            // Check if user already reported this post
            var existingReport = await _context.Reports
                .FirstOrDefaultAsync(r => r.PostId == createReportDto.PostId && r.ReporterId == reporterId);

            if (existingReport != null)
            {
                throw new InvalidOperationException("You have already reported this post");
            }

            var report = new Report
            {
                PostId = createReportDto.PostId,
                ReporterId = reporterId,
                Reason = createReportDto.Reason,
                Description = createReportDto.Description,
                Status = ReportStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return await ToDtoAsync(report);
        }

        public async Task<IEnumerable<ReportResponseDto>> GetReportsAsync(int pageNumber = 1, int pageSize = 20)
        {
            var reports = await _context.Reports
                .Include(r => r.Post)
                .Include(r => r.Reporter)
                .Include(r => r.Reviewer)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<ReportResponseDto>();
            foreach (var report in reports)
            {
                result.Add(await ToDtoAsync(report));
            }

            return result;
        }

        public async Task<IEnumerable<ReportResponseDto>> GetPendingReportsAsync(int pageNumber = 1, int pageSize = 20)
        {
            var reports = await _context.Reports
                .Where(r => r.Status == ReportStatus.Pending)
                .Include(r => r.Post)
                .Include(r => r.Reporter)
                .Include(r => r.Reviewer)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new List<ReportResponseDto>();
            foreach (var report in reports)
            {
                result.Add(await ToDtoAsync(report));
            }

            return result;
        }

        public async Task<ReportResponseDto?> GetReportByIdAsync(int id)
        {
            var report = await _context.Reports
                .Include(r => r.Post)
                .Include(r => r.Reporter)
                .Include(r => r.Reviewer)
                .FirstOrDefaultAsync(r => r.Id == id);

            return report == null ? null : await ToDtoAsync(report);
        }

        public async Task<bool> UpdateReportStatusAsync(int id, UpdateReportDto updateReportDto, string reviewerId)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return false;
            }

            report.Status = updateReportDto.Status;
            report.ActionTaken = updateReportDto.ActionTaken;
            report.ReviewedBy = reviewerId;
            report.ReviewedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReportAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return false;
            }

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetPendingReportsCountAsync()
        {
            return await _context.Reports.CountAsync(r => r.Status == ReportStatus.Pending);
        }

        private async Task<ReportResponseDto> ToDtoAsync(Report report)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == report.PostId);

            return new ReportResponseDto
            {
                Id = report.Id,
                PostId = report.PostId,
                ReporterId = report.ReporterId,
                ReporterDisplayName = report.Reporter?.UserName ?? "Unknown",
                Reason = report.Reason,
                Description = report.Description,
                Status = report.Status,
                ReviewedBy = report.ReviewedBy,
                ReviewerDisplayName = report.Reviewer?.UserName,
                ReviewedAt = report.ReviewedAt,
                ActionTaken = report.ActionTaken,
                CreatedAt = report.CreatedAt,
                PostContent = post?.Content ?? "Post not found",
                PostAuthorDisplayName = post?.User?.UserName ?? "Unknown",
                PostGameTitle = post?.GameTitle
            };
        }
    }
}