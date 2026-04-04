using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IReactionValidator
    {
        public Task<ValidationResult> CanCreateAsync(int currentUserId, CreateReactionDto dto, CancellationToken cancellationToken);
    }
}
