using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;
using MicrobloggingSystem.Models.ViewModels;

namespace MicrobloggingSystem.Controllers
{
    public class PostsMvcController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsMvcController(
            IPostService postService,
            ICommentService commentService,
            UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var posts = await _postService.GetPostsAsync(pageNumber, 20);
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var comments = await _commentService.GetCommentsForPostAsync(id);
            var model = new PostDetailsViewModel
            {
                Post = post,
                Comments = comments,
                NewComment = new CreateCommentDto { PostId = id }
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new CreatePostDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createPostDto);
            }

            createPostDto.UserId = _userManager.GetUserId(User) ?? string.Empty;
            var createdPost = await _postService.CreatePostAsync(createPostDto);
            return RedirectToAction(nameof(Details), new { id = createdPost.Id });
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!await CanEditPostAsync(post))
            {
                return Forbid();
            }

            var model = new UpdatePostDto
            {
                Content = post.Content,
                PostType = post.PostType,
                MediaPath = post.MediaPath,
                MediaType = post.MediaType
            };

            ViewBag.PostId = id;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PostId = id;
                return View(updatePostDto);
            }

            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!await CanEditPostAsync(post))
            {
                return Forbid();
            }

            var success = await _postService.UpdatePostAsync(id, updatePostDto);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (!await CanEditPostAsync(post))
            {
                return Forbid();
            }

            await _postService.DeletePostAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CanEditPostAsync(PostResponseDto post)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (string.Equals(currentUserId, post.UserId, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var currentUser = await _userManager.GetUserAsync(User);
            return currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");
        }
    }
}
