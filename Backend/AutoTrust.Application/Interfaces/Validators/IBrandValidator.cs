using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IBrandValidator
    {
        public Task<ValidationResult> CanCreateAsync(CreateBrandDto dto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanUpdateAsync(int id, UpdateBrandDto dto, CancellationToken cancellationToken);
    }
}
