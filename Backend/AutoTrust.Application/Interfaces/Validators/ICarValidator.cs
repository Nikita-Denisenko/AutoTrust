using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Common;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface ICarValidator
    {
        public Task<ValidationResult> CanCreate(CreateCarDto dto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanUpdate(int id, UpdateCarDto dto, CancellationToken cancellationToken);
    }
}
