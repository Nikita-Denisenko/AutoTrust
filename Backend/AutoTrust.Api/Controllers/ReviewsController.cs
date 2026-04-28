using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Review;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Review;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IReviewService _service;

        public ReviewsController(ICurrentUserService currentUser, IReviewService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(
            [FromBody] CreateReviewDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdReview = await _service.CreateReviewAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetUserReviews), new { userId = dto.ReceiverId }, createdReview);
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

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserReviews(
            [FromRoute] int userId,
            [FromQuery] ReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reviews = await _service.GetReviewsAsync(userId, filterDto, cancellationToken);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin/by-user/{userId:int}")]
        public async Task<IActionResult> GetReviewsByUserForAdmin(
            [FromRoute] int userId,
            [FromQuery] AdminReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reviews = await _service.GetReviewsByUserForAdminAsync(userId, filterDto, cancellationToken);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin/to-user/{userId:int}")]
        public async Task<IActionResult> GetReviewsToUserForAdmin(
            [FromRoute] int userId,
            [FromQuery] AdminReviewFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var reviews = await _service.GetReviewsToUserForAdminAsync(userId, filterDto, cancellationToken);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateReview(
            [FromRoute] int id,
            [FromBody] UpdateReviewDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateReviewAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Review with ID {id} was successfully updated.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
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
        public async Task<IActionResult> DeleteReview(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteReviewAsync(id, _currentUser.UserId!.Value, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
