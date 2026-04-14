using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PostResponseDto> Posts { get; set; } = Enumerable.Empty<PostResponseDto>();
        public bool IsAuthenticated { get; set; }
    }
}
