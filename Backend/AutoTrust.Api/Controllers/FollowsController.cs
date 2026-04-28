using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Follow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IFollowService _service;

        public FollowsController(ICurrentUserService currentUser, IFollowService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFollow(
            [FromBody] CreateFollowDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdFollow = await _service.CreateFollowAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetUserFollows), new { userId = _currentUser.UserId!.Value }, createdFollow);
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFollow(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteFollowAsync(id, _currentUser.UserId!.Value, cancellationToken);
                return NoContent();
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

        [HttpGet("followers/{userId:int}")]
        public async Task<IActionResult> GetUserFollowers(
            [FromRoute] int userId,
            [FromQuery] FollowFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var followers = await _service.GetUserFollowersAsync(userId, filterDto, cancellationToken);
                return Ok(followers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("follows/{userId:int}")]
        public async Task<IActionResult> GetUserFollows(
            [FromRoute] int userId,
            [FromQuery] FollowFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var follows = await _service.GetUserFollowsAsync(userId, filterDto, cancellationToken);
                return Ok(follows);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
