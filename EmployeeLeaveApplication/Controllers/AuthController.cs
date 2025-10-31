using EmployeeLeaveApplication.Models;
using EmployeeLeaveApplication.Services;
using EmployeeLeaveApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace EmployeeLeaveApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JsonServerService _jsonServerService;
        private readonly IConfiguration _config;

        public AuthController(JsonServerService jsonServerService, IConfiguration config)
        {
            _jsonServerService = jsonServerService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginUser)
        {
            var userData = await _jsonServerService.GetUserAsync(loginUser.UserId, true);

            if (userData == null)
                return Unauthorized("User not found");

            if (userData.Password != loginUser.Password)
                return Unauthorized("User Id and Password does not match");

            // Generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = _config.GetSection("Jwt:Key")?.Value;
            if (string.IsNullOrEmpty(jwtKey))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "JWT key configuration is missing.");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim("Role", userData.Role?.Name ?? string.Empty)
                    ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}
