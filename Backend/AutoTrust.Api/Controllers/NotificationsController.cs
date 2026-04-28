using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Notification;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Notification;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Notification;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly INotificationService _service;

        public NotificationsController(ICurrentUserService currentUser, INotificationService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateNotification(
            [FromBody] CreateNotificationDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdNotification = await _service.CreateNotificationAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetNotification), new { id = createdNotification.Id }, createdNotification);
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

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetNotification(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _service.GetNotificationAsync(id, _currentUser.UserId!.Value, cancellationToken);
                return Ok(notification);
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
        public async Task<IActionResult> GetNotificationForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var notification = await _service.GetNotificationForAdminAsync(id, cancellationToken);
                return Ok(notification);
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
        public async Task<IActionResult> GetNotifications(
            [FromQuery] NotificationFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _service.GetNotificationsAsync(_currentUser.UserId!.Value, filterDto, cancellationToken);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetNotificationsForAdmin(
            [FromQuery] AdminNotificationFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _service.GetNotificationsForAdminAsync(filterDto, cancellationToken);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateNotification(
            [FromRoute] int id,
            [FromBody] UpdateNotificationDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateNotificationAsync(id, dto, cancellationToken);
                return Ok($"Notification with ID {id} was successfully updated.");
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
        public async Task<IActionResult> MarkAsRead(
            [FromBody] MarkAsReadNotificationsDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.MarkAsReadNotificationsAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("Notifications marked as read successfully.");
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
        public async Task<IActionResult> DeleteNotifications(
            [FromBody] DeleteNotificationsDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteNotificationsAsync(_currentUser.UserId!.Value, false, dto, cancellationToken);
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

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("admin")]
        public async Task<IActionResult> DeleteNotificationsForAdmin(
            [FromBody] DeleteNotificationsDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteNotificationsAsync(_currentUser.UserId!.Value, true, dto, cancellationToken);
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
