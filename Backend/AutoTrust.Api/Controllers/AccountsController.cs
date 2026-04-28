using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Account;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Account;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAccountService _service;

        public AccountsController(ICurrentUserService currentUser, IAccountService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount(CancellationToken cancellationToken)
        {
            try
            {
                var account = await _service.GetUserAccountAsync(_currentUser.UserId!.Value, cancellationToken);
                return Ok(account);
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
        public async Task<IActionResult> GetAccountForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var account = await _service.GetAccountForAdminAsync(id, cancellationToken);
                return Ok(account);
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
        public async Task<IActionResult> GetAccountsForAdmin(
            [FromQuery] AdminAccountFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _service.GetAccountsForAdminAsync(filterDto, cancellationToken);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("me/email")]
        public async Task<IActionResult> ChangeMyEmail(
            [FromBody] ChangeEmailDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.ChangeEmailAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("Email updated successfully.");
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

        [HttpPatch("me/password")]
        public async Task<IActionResult> ChangeMyPassword(
            [FromBody] ChangePasswordDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.ChangePasswordAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("Password changed successfully.");
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

        [HttpPatch("me/phone")]
        public async Task<IActionResult> ChangeMyPhone(
            [FromBody] ChangePhoneDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.ChangePhoneAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return Ok("Phone number updated successfully.");
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
