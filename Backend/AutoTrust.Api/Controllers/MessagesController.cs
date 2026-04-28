using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Message;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Message;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Message;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IMessageService _service;

        public MessagesController(ICurrentUserService currentUser, IMessageService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(
            [FromBody] CreateMessageDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdMessage = await _service.CreateMessageAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetMessages), new { chatId = dto.ChatId }, createdMessage);
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

        [HttpGet("chat/{chatId:int}")]
        public async Task<IActionResult> GetMessages(
            [FromRoute] int chatId,
            [FromQuery] MessageFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _service.GetMessagesAsync(_currentUser.UserId!.Value, chatId, filterDto, cancellationToken);
                return Ok(messages);
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
        [HttpGet("admin")]
        public async Task<IActionResult> GetMessagesForAdmin(
            [FromQuery] AdminMessageFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var messages = await _service.GetMessagesForAdminAsync(filterDto, cancellationToken);
                return Ok(messages);
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

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateMessage(
            [FromRoute] int id,
            [FromBody] UpdateMessageDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateMessageAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Message with ID {id} was successfully updated.");
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

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkMessagesAsRead(
            [FromBody] MarkAsReadMessagesDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.MarkMessagesAsReadAsync(_currentUser.UserId!.Value, dto.ChatId, dto, cancellationToken);
                return Ok("Messages marked as read successfully.");
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

        [HttpDelete]
        public async Task<IActionResult> DeleteMessages(
            [FromBody] DeleteMessagesDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteMessagesAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return NoContent();
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
