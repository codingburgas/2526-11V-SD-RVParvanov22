using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Models.ViewModels
{
    public class ReportsViewModel
    {
        public IEnumerable<TopUserViewModel> TopUsers { get; set; } = Enumerable.Empty<TopUserViewModel>();
        public PostResponseDto? PopularPost { get; set; }
        public double AveragePostsPerUser { get; set; }
    }
}
