using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsApiController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsApiController(
            IReportService reportService,
            UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReportDto createReportDto)
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var report = await _reportService.CreateReportAsync(createReportDto, userId);
                return Ok(new { message = "Report submitted successfully", report });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}