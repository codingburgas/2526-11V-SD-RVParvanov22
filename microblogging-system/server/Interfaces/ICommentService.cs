using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentResponseDto>> GetCommentsForPostAsync(int postId);
        Task<CommentResponseDto?> GetCommentByIdAsync(int id);
        Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto createCommentDto);
        Task<bool> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto);
        Task<bool> DeleteCommentAsync(int id);
    }
}
