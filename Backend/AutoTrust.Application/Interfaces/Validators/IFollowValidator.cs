using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IFollowValidator
    {
        public Task<ValidationResult> CanCreateAsync(int currentUserId, CreateFollowDto dto, CancellationToken cancellationToken);
    }
}
