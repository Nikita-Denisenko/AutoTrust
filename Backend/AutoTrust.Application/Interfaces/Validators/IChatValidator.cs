using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IChatValidator
    {
        public Task<ValidationResult> CanCreate(CreateChatDto dto, int currentUserId, CancellationToken cancellationToken);
    }
}
