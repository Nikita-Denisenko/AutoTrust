using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Car;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly ICarService _service;

        public CarsController(ICurrentUserService currentUser, ICarService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(
            [FromBody] CreateCarDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdCar = await _service.CreateCarAsync(_currentUser.UserId!.Value, dto, cancellationToken);
                return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, createdCar);
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
        public async Task<IActionResult> GetCar(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var car = await _service.GetCarAsync(id, cancellationToken);
                return Ok(car);
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
        public async Task<IActionResult> GetCarForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var car = await _service.GetCarForAdminAsync(id, cancellationToken);
                return Ok(car);
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
        public async Task<IActionResult> GetCars(
            [FromQuery] CarFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var cars = await _service.GetCarsAsync(filterDto, _currentUser.UserId!.Value, cancellationToken);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetCarsForAdmin(
            [FromQuery] AdminCarFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var cars = await _service.GetCarsForAdminAsync(filterDto, cancellationToken);
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateCar(
            [FromRoute] int id,
            [FromBody] UpdateCarDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateCarAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Car with ID {id} was successfully updated!");
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
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCar([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteCarAsync(id, _currentUser.UserId!.Value, cancellationToken);
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

        [HttpPost("make-for-sale/{id:int}")]
        public async Task<IActionResult> MakeForSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _service.MakeForSaleAsync(id, _currentUser.UserId!.Value, cancellationToken);
                return Ok($"Car with ID {id} is now on sale");
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

        [HttpPost("take-off-sale/{id:int}")]
        public async Task<IActionResult> TakeOffSale([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                await _service.TakeOffSaleAsync(id, _currentUser.UserId!.Value, cancellationToken);
                return Ok($"Car with ID {id} has been removed from sale");
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

        [HttpPost("transfer-ownership/{id:int}")]
        public async Task<IActionResult> TransferOwnership(
            [FromRoute] int id,
            [FromBody] TransferOwnershipDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.TransferOwnershipAsync(id, _currentUser.UserId!.Value, dto, cancellationToken);
                return Ok($"Car with ID {id} has been successfully transferred to user with ID {dto.NewOwnerId}");
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