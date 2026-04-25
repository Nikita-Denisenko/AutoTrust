using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.AuthDtos;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto, cancellationToken);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.LoginAsync(dto, cancellationToken);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }
    }
}