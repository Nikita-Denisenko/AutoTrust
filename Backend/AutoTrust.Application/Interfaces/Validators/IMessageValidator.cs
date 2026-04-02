using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IMessageValidator
    {
        public Task<ValidationResult> CanCreateAsync(int currentUserId, CreateMessageDto dto, CancellationToken cancellationToken);
    }
}
