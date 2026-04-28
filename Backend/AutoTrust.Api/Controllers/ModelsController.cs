using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model;
using AutoTrust.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoTrust.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ModelsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IModelService _service;

        public ModelsController(ICurrentUserService currentUser, IModelService service)
        {
            _currentUser = currentUser;
            _service = service;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateModel(
            [FromBody] CreateModelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var createdModel = await _service.CreateModelAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetModel), new { id = createdModel.Id }, createdModel);
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
        public async Task<IActionResult> GetModel(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var model = await _service.GetModelAsync(id, cancellationToken);
                return Ok(model);
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
        public async Task<IActionResult> GetModelForAdmin(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                var model = await _service.GetModelForAdminAsync(id, cancellationToken);
                return Ok(model);
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
        public async Task<IActionResult> GetModels(
            [FromQuery] ModelFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var models = await _service.GetModelsAsync(filterDto, cancellationToken);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("admin")]
        public async Task<IActionResult> GetModelsForAdmin(
            [FromQuery] AdminModelFilterDto filterDto,
            CancellationToken cancellationToken)
        {
            try
            {
                var models = await _service.GetModelsForAdminAsync(filterDto, cancellationToken);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPatch("{id:int}/rename")]
        public async Task<IActionResult> RenameModel(
            [FromRoute] int id,
            [FromBody] RenameModelDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.RenameModelAsync(id, dto, cancellationToken);
                return Ok($"Model with ID {id} was successfully renamed.");
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
        [HttpPatch("{id:int}/description")]
        public async Task<IActionResult> UpdateModelDescription(
            [FromRoute] int id,
            [FromBody] UpdateModelDescriptionDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateModelDescriptionAsync(id, dto, cancellationToken);
                return Ok($"Model description with ID {id} was successfully updated.");
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
        [HttpPatch("{id:int}/image")]
        public async Task<IActionResult> UpdateModelImage(
            [FromRoute] int id,
            [FromBody] UpdateModelImageDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.UpdateModelImageAsync(id, dto, cancellationToken);
                return Ok($"Model image with ID {id} was successfully updated.");
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
        public async Task<IActionResult> DeleteModel(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            try
            {
                await _service.DeleteModelAsync(id, cancellationToken);
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
