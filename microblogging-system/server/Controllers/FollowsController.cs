using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs;

namespace MicrobloggingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FollowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("followers/{userId}")]
        public async Task<ActionResult<IEnumerable<FollowResponseDto>>> GetFollowers(string userId)
        {
            var followers = await _context.Follows
                .Where(f => f.FollowingId == userId)
                .ToListAsync();

            return Ok(followers.Select(f => new FollowResponseDto
            {
                Id = f.Id,
                FollowerId = f.FollowerId,
                FollowingId = f.FollowingId,
                CreatedAt = f.CreatedAt
            }));
        }

        [HttpGet("following/{userId}")]
        public async Task<ActionResult<IEnumerable<FollowResponseDto>>> GetFollowing(string userId)
        {
            var following = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .ToListAsync();

            return Ok(following.Select(f => new FollowResponseDto
            {
                Id = f.Id,
                FollowerId = f.FollowerId,
                FollowingId = f.FollowingId,
                CreatedAt = f.CreatedAt
            }));
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FollowResponseDto>> Follow([FromBody] CreateFollowDto createFollowDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createFollowDto.FollowerId == createFollowDto.FollowingId)
            {
                return BadRequest(new { error = "A user cannot follow themselves" });
            }

            var existingFollow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == createFollowDto.FollowerId && f.FollowingId == createFollowDto.FollowingId);

            if (existingFollow != null)
            {
                return Conflict(new { error = "Already following this user" });
            }

            var user = await _context.Users.FindAsync(createFollowDto.FollowingId);
            if (user == null)
            {
                return BadRequest(new { error = "User to follow not found" });
            }

            var follow = new Follow
            {
                FollowerId = createFollowDto.FollowerId,
                FollowingId = createFollowDto.FollowingId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Follows.Add(follow);
            await _context.SaveChangesAsync();

            var response = new FollowResponseDto
            {
                Id = follow.Id,
                FollowerId = follow.FollowerId,
                FollowingId = follow.FollowingId,
                CreatedAt = follow.CreatedAt
            };

            return CreatedAtAction(nameof(GetFollowers), new { userId = follow.FollowingId }, response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(int id)
        {
            var follow = await _context.Follows.FindAsync(id);
            if (follow == null)
            {
                return NotFound(new { error = "Follow record not found" });
            }

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
