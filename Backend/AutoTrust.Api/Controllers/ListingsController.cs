using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListingsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IListingService _service;

        public ListingsController(ICurrentUserService currentUser, IListingService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> CreateBuyListing(
            [FromBody] CreateBuyListingDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdListing = await _service.CreateBuyListingAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetUserBuyListings), new { userId = _currentUser.UserId!.Value }, createdListing);
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

        [HttpPost("sale")]
        public async Task<IActionResult> CreateSaleListing(
            [FromBody] CreateSaleListingDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdListing = await _service.CreateSaleListingAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetUserSaleListings), new { userId = _currentUser.UserId!.Value }, createdListing);
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

        [HttpGet("feed")]
        public async Task<IActionResult> GetFeedListings(
            [FromQuery] FeedListingFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var listings = await _service.GetFeedListingsAsync(filterDto, cancellationToken);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId:int}/buy")]
        public async Task<IActionResult> GetUserBuyListings(
            [FromRoute] int userId,
            [FromQuery] BuyListingFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var listings = await _service.GetUserBuyListingsAsync(userId, filterDto, cancellationToken);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId:int}/sale")]
        public async Task<IActionResult> GetUserSaleListings(
            [FromRoute] int userId,
            [FromQuery] SaleListingFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var listings = await _service.GetUserSaleListingsAsync(userId, filterDto, cancellationToken);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetListingsForAdmin(
            [FromQuery] AdminListingFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var listings = await _service.GetListingsForAdminAsync(filterDto, cancellationToken);
                return Ok(listings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}/info")]
        public async Task<IActionResult> UpdateListingInfo(
            [FromRoute] int id,
            [FromBody] UpdateListingInfoDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateListingInfoAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Listing with ID {id} was successfully updated.");
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

        [HttpPatch("{id:int}/buy")]
        public async Task<IActionResult> UpdateBuyListing(
            [FromRoute] int id,
            [FromBody] UpdateBuyListingDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateBuyListingAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Buy listing with ID {id} was successfully updated.");
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

        [HttpPatch("{id:int}/sale")]
        public async Task<IActionResult> UpdateSaleListing(
            [FromRoute] int id,
            [FromBody] UpdateSaleListingDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateSaleListingAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Sale listing with ID {id} was successfully updated.");
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
        [HttpPatch("{id:int}/activate")]
        public async Task<IActionResult> ActivateListing(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.ActivateListingAsync(id, cancellationToken);
                return Ok($"Listing with ID {id} was successfully activated.");
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
        [HttpPatch("{id:int}/deactivate")]
        public async Task<IActionResult> DeactivateListing(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeactivateListingAsync(id, cancellationToken);
                return Ok($"Listing with ID {id} was successfully deactivated.");
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteListing(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteListingAsync(id, _currentUser.UserId!.Value, cancellationToken);
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
