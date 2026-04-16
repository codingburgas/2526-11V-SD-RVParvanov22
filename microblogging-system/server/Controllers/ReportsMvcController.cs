using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicrobloggingSystem.Interfaces;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsMvcController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IPostService _postService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsMvcController(
            IReportService reportService,
            IPostService postService,
            UserManager<ApplicationUser> userManager)
        {
            _reportService = reportService;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            var reports = await _reportService.GetReportsAsync(pageNumber, 20);
            var pendingCount = await _reportService.GetPendingReportsCountAsync();
            ViewBag.PendingCount = pendingCount;
            return View(reports);
        }

        public async Task<IActionResult> Pending(int pageNumber = 1)
        {
            var reports = await _reportService.GetPendingReportsAsync(pageNumber, 20);
            return View("Index", reports);
        }

        public async Task<IActionResult> Details(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> Review(int id, Models.ReportStatus status, Models.ReportAction action)
        {
            var updateDto = new UpdateReportDto
            {
                Status = status,
                ActionTaken = action
            };

            var reviewerId = _userManager.GetUserId(User) ?? string.Empty;
            var success = await _reportService.UpdateReportStatusAsync(id, updateDto, reviewerId);

            if (!success)
            {
                return NotFound();
            }

            TempData["Message"] = "Report has been reviewed successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reportService.DeleteReportAsync(id);
            if (!success)
            {
                return NotFound();
            }

            TempData["Message"] = "Report has been deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}