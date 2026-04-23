using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PostResponseDto>> GetPostsAsync(int pageNumber, int pageSize)
        {
            // Public feed: only published posts, newest first.
            var posts = await _context.Posts
                .Where(p => !p.IsDraft) // Only show published posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return posts.Select(post => ToDto(post)).ToList();
        }

        public async Task<PostResponseDto?> GetPostByIdAsync(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .FirstOrDefaultAsync(p => p.Id == id);

            return post == null ? null : ToDto(post);
        }

        public async Task<PostResponseDto> CreatePostAsync(CreatePostDto createPostDto)
        {
            var user = await _context.Users.FindAsync(createPostDto.UserId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            // Drafts are saved without PublishedAt; published posts get a timestamp immediately.
            var post = new Post
            {
                GameTitle = createPostDto.GameTitle,
                Content = createPostDto.Content,
                PostType = createPostDto.PostType,
                MediaPath = createPostDto.MediaPath,
                MediaType = createPostDto.MediaType,
                UserId = createPostDto.UserId,
                IsDraft = createPostDto.IsDraft,
                CreatedAt = DateTime.UtcNow,
                PublishedAt = createPostDto.IsDraft ? null : DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return ToDto(post, user);
        }

        public async Task<bool> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }

            var wasDraft = post.IsDraft;
            post.Content = updatePostDto.Content;
            post.GameTitle = updatePostDto.GameTitle;
            post.PostType = updatePostDto.PostType ?? post.PostType;
            post.MediaPath = updatePostDto.MediaPath ?? post.MediaPath;
            post.MediaType = updatePostDto.MediaType ?? post.MediaType;
            post.IsDraft = updatePostDto.IsDraft;

            // Preserve the original publish moment once a draft becomes visible in the feed.
            if (wasDraft && !updatePostDto.IsDraft && post.PublishedAt == null)
            {
                post.PublishedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePostAsync(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PostResponseDto>> GetFeedAsync(string userId, int pageNumber, int pageSize)
        {
            // Personalized feed is built from followed authors plus the current user's own posts.
            var followedIds = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToListAsync();

            if (followedIds.Count == 0)
            {
                // Fall back to the global feed when the user follows nobody yet.
                return await GetPostsAsync(pageNumber, pageSize);
            }

            var posts = await _context.Posts
                .Where(p => (followedIds.Contains(p.UserId) || p.UserId == userId) && !p.IsDraft)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return posts.Select(post => ToDto(post)).ToList();
        }

        public async Task<IEnumerable<PostResponseDto>> SearchPostsAsync(string query, string? gameTitle = null, string? postType = null, int pageNumber = 1, int pageSize = 20)
        {
            var postsQuery = _context.Posts
                .Where(p => !p.IsDraft) // Only search published posts by default
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .AsQueryable();

            // Filters are applied incrementally so the same method can serve simple and advanced search.
            if (!string.IsNullOrWhiteSpace(query))
            {
                postsQuery = postsQuery.Where(p =>
                    p.Content.Contains(query) ||
                    p.GameTitle.Contains(query) ||
                    (p.User != null && (p.User.DisplayName.Contains(query) || p.User.UserName.Contains(query)))
                );
            }

            if (!string.IsNullOrWhiteSpace(gameTitle))
            {
                postsQuery = postsQuery.Where(p => p.GameTitle.Contains(gameTitle));
            }

            if (!string.IsNullOrWhiteSpace(postType))
            {
                postsQuery = postsQuery.Where(p => p.PostType == postType);
            }

            var posts = await postsQuery
                .OrderByDescending(p => p.PublishedAt ?? p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return posts.Select(post => ToDto(post)).ToList();
        }

        public async Task<IEnumerable<PostResponseDto>> GetDraftsAsync(string userId, int pageNumber = 1, int pageSize = 20)
        {
            // Drafts stay private to their owner and are ordered by last creation time.
            var drafts = await _context.Posts
                .Where(p => p.UserId == userId && p.IsDraft)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return drafts.Select(post => ToDto(post)).ToList();
        }

        public async Task<bool> PublishDraftAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null || !post.IsDraft)
            {
                return false;
            }

            // Publishing a draft moves it into the main feed.
            post.IsDraft = false;
            post.PublishedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private static PostResponseDto ToDto(Post post, ApplicationUser? user = null)
        {
            if (user == null)
            {
                user = post.User;
            }

            // DTOs expose only client-safe data and hide full entity graphs.
            return new PostResponseDto
            {
                Id = post.Id,
                GameTitle = post.GameTitle,
                Content = post.Content,
                PostType = post.PostType,
                MediaPath = post.MediaPath,
                MediaType = post.MediaType,
                CreatedAt = post.CreatedAt,
                PublishedAt = post.PublishedAt,
                IsDraft = post.IsDraft,
                CommentsCount = post.Comments?.Count ?? 0,
                LikesCount = post.PostLikes?.Count ?? 0,
                UserId = post.UserId,
                UserDisplayName = user?.DisplayName ?? user?.UserName,
                UserProfilePictureUrl = user?.ProfilePictureUrl
            };
        }
    }
}
