using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IModelValidator
    {
        public Task<ValidationResult> CanCreateAsync(CreateModelDto dto, CancellationToken cancellationToken);
    }
}
