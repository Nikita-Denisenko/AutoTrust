using AutoTrust.Application.Common;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface ISaleListingValidator
    {
        public Task<ValidationResult> IsSaleValidAsync(int userId, int carId, CancellationToken cancellationToken);
    }
}
