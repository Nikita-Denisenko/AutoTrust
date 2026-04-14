using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IModelService
    {
        public Task<CreatedModelDto> CreateModelAsync(CreateModelDto dto, CancellationToken cancellationToken);
        public Task<ModelDto> GetModelAsync(int id, CancellationToken cancellationToken);
        public Task<AdminModelDto> GetModelForAdminAsync(int id, CancellationToken cancellationToken);
        public Task<List<ModelListItemDto>> GetModelsAsync(ModelFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminModelListItemDto>> GetModelsForAdminAsync(AdminModelFilterDto adminFilterDto, CancellationToken cancellationToken);
        public Task RenameModelAsync(int id, RenameModelDto dto, CancellationToken cancellationToken);
        public Task UpdateModelImageAsync(int id, UpdateModelImageDto dto, CancellationToken cancellationToken);
        public Task UpdateModelDescriptionAsync(int id, UpdateModelDescriptionDto dto, CancellationToken cancellationToken);
        public Task DeleteModelAsync(int id, CancellationToken cancellationToken);
    }
}
