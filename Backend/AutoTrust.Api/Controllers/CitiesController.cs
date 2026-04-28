using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.City;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _service;

        public CitiesController(ICityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(
            [FromQuery] CityFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var cities = await _service.GetCitiesAsync(filterDto, cancellationToken);
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}