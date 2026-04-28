using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Comment;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Comment;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ICommentService _service;

        public CommentsController(ICurrentUserService currentUser, ICommentService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(
            [FromBody] CreateCommentDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdComment = await _service.CreateCommentAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetComments), new { listingId = dto.ListingId }, createdComment);
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
        public async Task<IActionResult> GetComments(
            [FromRoute] int listingId,
            [FromQuery] CommentFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var comments = await _service.GetCommentsAsync(listingId, filterDto, cancellationToken);
                return Ok(comments);
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
        [HttpGet("admin")]
        public async Task<IActionResult> GetCommentsForAdmin(
            [FromQuery] AdminCommentFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var comments = await _service.GetCommentsForAdminAsync(filterDto, cancellationToken);
                return Ok(comments);
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

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] int id,
            [FromBody] UpdateCommentDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateCommentAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Comment with ID {id} was successfully updated.");
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteCommentAsync(id, _currentUser.UserId!.Value, cancellationToken);
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

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{id:int}/block")]
        public async Task<IActionResult> BlockComment(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.BlockCommentByAdminAsync(id, cancellationToken);
                return Ok($"Comment with ID {id} was successfully blocked.");
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
        public async Task<IActionResult> UnblockComment(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UnblockCommentByAdminAsync(id, cancellationToken);
                return Ok($"Comment with ID {id} was successfully unblocked.");
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