using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Identity;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Infrastructure.Services;

namespace TaskManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              IConfiguration configuration,
                              IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
                return BadRequest("Passwords do not match");

            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            // Generate a confirmation link using Url.Action (if available) or build manually.
            // Note: In WebAPI, Url.Action might require additional configuration (or you may build the URL manually)
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { userId = user.Id, token = token }, Request.Scheme);
            // Send email with confirmation link
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking on the following link: {confirmationLink}");

            return Ok("User registered successfully. Please check your email to confirm your account.");
        }

        [HttpPost("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("Invalid User ID");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully");
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, dto.Password)))
            {
                return Unauthorized("Invalid credentials");
            }

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Ok(new { Token = jwtToken });
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
                return Ok("Password changed successfully");
            return BadRequest(result.Errors);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendEmailAsync(email, "Reset your password", $"Your password reset token is: {token}");
            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest("User not found");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                return Ok("Password reset successfully");
            return BadRequest(result.Errors);
        }

        [HttpPost("sendverificationcode")]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendVerificationDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return NotFound("User not found.");

            // Generate a random 6-digit code.
            var code = new Random().Next(100000, 999999).ToString();
            await _emailService.SendVerificationCodeAsync(dto.Email, code);

            // For demonstration, return the code (DO NOT do this in production)
            return Ok(new { Message = "Verification code sent.", Code = code });
        }
    }
}
