using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicrobloggingSystem.Data;
using MicrobloggingSystem.Models;
using MicrobloggingSystem.Models.DTOs.Auth;
using MicrobloggingSystem.Services;

namespace MicrobloggingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Invalid input data",
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { error = "User with this email already exists" });
                }

                // Create new user
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    DisplayName = registerDto.DisplayName,
                    Region = registerDto.Region,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        error = "Failed to create user",
                        details = result.Errors.Select(e => e.Description).ToList()
                    });
                }

                _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);

                // Generate JWT token
                var token = _jwtService.GenerateToken(user);

                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Region = user.Region,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // Match JWT expiry
                };

                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new { error = "An error occurred during registration" });
            }
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        error = "Invalid input data",
                        details = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });
                }

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new { error = "Invalid email or password" });
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded)
                {
                    return Unauthorized(new { error = "Invalid email or password" });
                }

                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);

                // Generate JWT token
                var token = _jwtService.GenerateToken(user);

                var response = new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Region = user.Region,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60) // Match JWT expiry
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user login");
                return StatusCode(500, new { error = "An error occurred during login" });
            }
        }

        /// <summary>
        /// Google OAuth login (placeholder/scaffolded)
        /// </summary>
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            // TODO: Implement Google OAuth
            return Ok(new
            {
                message = "Google authentication is not yet implemented",
                status = "placeholder",
                redirectUrl = "https://accounts.google.com/oauth/authorize?client_id=placeholder"
            });
        }

        /// <summary>
        /// Riot OAuth login (placeholder/scaffolded)
        /// </summary>
        [HttpGet("riot-login")]
        public IActionResult RiotLogin()
        {
            // TODO: Implement Riot OAuth
            return Ok(new
            {
                message = "Riot authentication is not yet implemented",
                status = "placeholder",
                note = "Coming soon - requires Riot developer account setup"
            });
        }
    }
}
