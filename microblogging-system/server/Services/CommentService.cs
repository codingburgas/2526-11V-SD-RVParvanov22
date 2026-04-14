using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsForPostAsync(int postId)
        {
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return comments.Select(c => new CommentResponseDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                UserDisplayName = c.User?.DisplayName ?? c.User?.UserName,
                PostId = c.PostId
            }).ToList();
        }

        public async Task<CommentResponseDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return null;
            }

            return new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                UserDisplayName = comment.User?.DisplayName ?? comment.User?.UserName,
                PostId = comment.PostId
            };
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var user = await _context.Users.FindAsync(createCommentDto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var post = await _context.Posts.FindAsync(createCommentDto.PostId);
            if (post == null)
            {
                throw new InvalidOperationException("Post not found");
            }

            var comment = new Comment
            {
                Content = createCommentDto.Content,
                UserId = createCommentDto.UserId,
                PostId = createCommentDto.PostId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
                UserDisplayName = user.DisplayName ?? user.UserName,
                PostId = comment.PostId
            };
        }

        public async Task<bool> UpdateCommentAsync(int id, UpdateCommentDto updateCommentDto)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            comment.Content = updateCommentDto.Content;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
