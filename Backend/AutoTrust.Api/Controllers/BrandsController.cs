using AutoTrust.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoTrust.Domain.Constants;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;


namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandsController(IBrandService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var createdBrand = await _service.CreateBrandAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetBrand), createdBrand.Id, createdBrand);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBrand([FromRoute] int id, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _service.GetBrandAsync(id, cancellationToken);
                return Ok(brand);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpGet("admin/{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetBrandForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _service.GetBrandForAdminAsync(id, cancellationToken);
                return Ok(brand);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBrands(
            [FromQuery] BrandFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var brands = await _service.GetBrandsAsync(filterDto, cancellationToken);
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpGet("admin")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetBrandsForAdmin(
            [FromQuery] AdminBrandFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var brands = await _service.GetBrandsForAdminAsync(filterDto, cancellationToken);
                return Ok(brands);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateBrand(
            [FromRoute] int id, 
            [FromBody] UpdateBrandDto dto, 
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateBrandAsync(id, dto, cancellationToken);
                return Ok($"Brand with ID {id} was successfully updated.");
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
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteBrand(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteBrandAsync(id, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server error: {ex.Message}");
            }
        }
    }
}
