using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Controllers
{
    public class CommentMvcController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentMvcController(
            ICommentService commentService,
            UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentDto createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "PostsMvc", new { id = createCommentDto.PostId });
            }

            createCommentDto.UserId = _userManager.GetUserId(User) ?? string.Empty;
            await _commentService.CreateCommentAsync(createCommentDto);
            return RedirectToAction("Details", "PostsMvc", new { id = createCommentDto.PostId });
        }
    }
}
