using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.User;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.User;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _service;

        public UsersController(ICurrentUserService currentUser, IUserService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            try
            {
                var user = await _service.GetUserByIdAsync(_currentUser.UserId!.Value, cancellationToken);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = await _service.GetUserByIdAsync(id, cancellationToken);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin/{id:int}")]
        public async Task<IActionResult> GetUserByIdForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var user = await _service.GetUserByIdForAdminAsync(id, cancellationToken);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] UserFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var users = await _service.GetUsersAsync(filterDto, cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetUsersForAdmin(
            [FromQuery] AdminUserFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var users = await _service.GetUsersForAdminAsync(filterDto, cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("me/avatar")]
        public async Task<IActionResult> UpdateMyAvatar(
            [FromBody] UpdateAvatarUrlDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateUserAvatarAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("Avatar updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("me/info")]
        public async Task<IActionResult> UpdateMyInfo(
            [FromBody] UpdateUserInfoDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateUserInfoAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("User info updated successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMyProfile(CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteUserProfileAsync(_currentUser.UserId!.Value, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserProfile(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteUserProfileAsync(id, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{id:int}/block")]
        public async Task<IActionResult> BlockUser(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.BlockUserAsync(id, cancellationToken);
                return Ok($"User with ID {id} has been blocked.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{id:int}/unblock")]
        public async Task<IActionResult> UnblockUser(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UnblockUserAsync(id, cancellationToken);
                return Ok($"User with ID {id} has been unblocked.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}