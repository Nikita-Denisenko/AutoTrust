using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Reaction;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReactionsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IReactionService _service;

        public ReactionsController(ICurrentUserService currentUser, IReactionService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction(
            [FromBody] CreateReactionDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdReaction = await _service.CreatedReactionAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetReactions), new { listingId = dto.ListingId }, createdReaction);
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

        [HttpGet("listing/{listingId:int}")]
        public async Task<IActionResult> GetReactions(
            [FromRoute] int listingId,
            [FromQuery] ReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reactions = await _service.GetReactionsAsync(listingId, filterDto, cancellationToken);
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("listing/{listingId:int}/admin")]
        public async Task<IActionResult> GetListingReactionsForAdmin(
            [FromRoute] int listingId,
            [FromQuery] AdminReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reactions = await _service.GetListingReactionsAsync(listingId, filterDto, cancellationToken);
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserReactionsForAdmin(
            [FromRoute] int userId,
            [FromQuery] AdminReactionFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reactions = await _service.GetUserReactionsAsync(userId, filterDto, cancellationToken);
                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteReaction(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteReactionAsync(id, _currentUser.UserId!.Value, cancellationToken);
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
    }
}
