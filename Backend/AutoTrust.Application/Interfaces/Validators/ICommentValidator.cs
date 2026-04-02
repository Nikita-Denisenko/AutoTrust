using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface ICommentValidator
    {
        public Task<ValidationResult> CanCreateAsync(CreateCommentDto dto, CancellationToken cancellationToken);
    }
}
