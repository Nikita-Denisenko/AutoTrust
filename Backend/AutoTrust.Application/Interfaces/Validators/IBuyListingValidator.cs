using AutoTrust.Application.Common;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;


namespace AutoTrust.Application.Interfaces.Validators
{
    public interface IBuyListingValidator
    {
        public Task<ValidationResult> CanCreateAsync(CreateBuyListingDto dto, CancellationToken cancellationToken);
        public Task<ValidationResult> CanUpdateAsync(UpdateBuyListingDto dto, CancellationToken cancellationToken)
    }
}
