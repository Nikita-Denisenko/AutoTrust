using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IChatService _service;

        public ChatsController(ICurrentUserService currentUser, IChatService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChat(
            [FromBody] CreateChatDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdChat = await _service.CreateChatAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetChatInfo), new { chatId = createdChat.Id }, createdChat);
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

        [HttpGet("{chatId:int}/info")]
        public async Task<IActionResult> GetChatInfo(
            [FromRoute] int chatId,
            CancellationToken cancellationToken)
        {
            try
            {
                var chatInfo = await _service.GetChatInfoAsync(_currentUser.UserId!.Value, chatId, cancellationToken);
                return Ok(chatInfo);
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

        [HttpGet]
        public async Task<IActionResult> GetUserChats(CancellationToken cancellationToken)
        {
            try
            {
                var chats = await _service.GetUserChatsAsync(_currentUser.UserId!.Value, cancellationToken);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin/user/{userId:int}")]
        public async Task<IActionResult> GetChatsForAdmin(
            [FromRoute] int userId,
            CancellationToken cancellationToken)
        {
            try
            {
                var chats = await _service.GetChatsForAdminAsync(userId, cancellationToken);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin/{chatId:int}")]
        public async Task<IActionResult> GetChatForAdmin(
            [FromRoute] int chatId,
            CancellationToken cancellationToken)
        {
            try
            {
                var chat = await _service.GetChatForAdminAsync(chatId, cancellationToken);
                return Ok(chat);
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
        [HttpDelete("admin/{chatId:int}")]
        public async Task<IActionResult> DeleteChatForAdmin(
            [FromRoute] int chatId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteChatForAdminAsync(chatId, cancellationToken);
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

        [HttpPost("{chatId:int}/pin/{messageId:int}")]
        public async Task<IActionResult> PinMessage(
            [FromRoute] int chatId,
            [FromRoute] int messageId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.PinMessageAsync(_currentUser.UserId!.Value, messageId, chatId, cancellationToken);
                return Ok($"Message with ID {messageId} has been pinned in chat with ID {chatId}");
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

        [HttpDelete("{chatId:int}/pin")]
        public async Task<IActionResult> UnpinMessage(
            [FromRoute] int chatId,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UnpinMessageAsync(chatId, cancellationToken);
                return Ok($"Pinned message has been unpinned in chat with ID {chatId}");
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