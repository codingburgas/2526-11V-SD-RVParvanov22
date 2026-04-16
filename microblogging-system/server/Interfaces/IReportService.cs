using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Interfaces
{
    public interface IReportService
    {
        Task<ReportResponseDto> CreateReportAsync(CreateReportDto createReportDto, string reporterId);
        Task<IEnumerable<ReportResponseDto>> GetReportsAsync(int pageNumber = 1, int pageSize = 20);
        Task<IEnumerable<ReportResponseDto>> GetPendingReportsAsync(int pageNumber = 1, int pageSize = 20);
        Task<ReportResponseDto?> GetReportByIdAsync(int id);
        Task<bool> UpdateReportStatusAsync(int id, UpdateReportDto updateReportDto, string reviewerId);
        Task<bool> DeleteReportAsync(int id);
        Task<int> GetPendingReportsCountAsync();
    }
}