using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Models.ViewModels
{
    public class PostDetailsViewModel
    {
        public PostResponseDto Post { get; set; } = new PostResponseDto();
        public IEnumerable<CommentResponseDto> Comments { get; set; } = Enumerable.Empty<CommentResponseDto>();
        public CreateCommentDto NewComment { get; set; } = new CreateCommentDto();
    }
}
