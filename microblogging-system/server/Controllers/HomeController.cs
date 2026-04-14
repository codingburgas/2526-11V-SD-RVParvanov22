using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;
using MicrobloggingSystem.Models.ViewModels;

namespace MicrobloggingSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            IPostService postService,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            IEnumerable<PostResponseDto> posts;
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User) ?? string.Empty;
                posts = await _postService.GetFeedAsync(userId, pageNumber, 20);
            }
            else
            {
                posts = await _postService.GetPostsAsync(pageNumber, 20);
            }

            var model = new HomeViewModel
            {
                Posts = posts,
                IsAuthenticated = User.Identity?.IsAuthenticated == true
            };

            return View(model);
        }

        public async Task<IActionResult> Reports()
        {
            var topUsers = await _context.Users
                .Select(u => new TopUserViewModel
                {
                    UserId = u.Id,
                    DisplayName = u.DisplayName ?? u.UserName ?? "Unknown",
                    FollowersCount = _context.Follows.Count(f => f.FollowingId == u.Id)
                })
                .OrderByDescending(u => u.FollowersCount)
                .Take(5)
                .ToListAsync();

            var popularPost = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.PostLikes)
                .Include(p => p.User)
                .OrderByDescending(p => (p.Comments!.Count + p.PostLikes!.Count))
                .FirstOrDefaultAsync();

            var averagePosts = await _context.Posts
                .GroupBy(p => p.UserId)
                .Select(g => g.Count())
                .DefaultIfEmpty(0)
                .AverageAsync();

            var model = new ReportsViewModel
            {
                TopUsers = topUsers,
                PopularPost = popularPost == null ? null : new PostResponseDto
                {
                    Id = popularPost.Id,
                    Content = popularPost.Content,
                    PostType = popularPost.PostType,
                    MediaPath = popularPost.MediaPath,
                    MediaType = popularPost.MediaType,
                    CreatedAt = popularPost.CreatedAt,
                    CommentsCount = popularPost.Comments?.Count ?? 0,
                    LikesCount = popularPost.PostLikes?.Count ?? 0,
                    UserId = popularPost.UserId,
                    UserDisplayName = popularPost.User?.DisplayName ?? popularPost.User?.UserName,
                    UserProfilePictureUrl = popularPost.User?.ProfilePictureUrl
                },
                AveragePostsPerUser = averagePosts
            };

            return View(model);
        }
    }
}
