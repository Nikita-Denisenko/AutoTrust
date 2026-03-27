using AutoTrust.Application.Common;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface ISaleListingValidator
    {
        public Task<ValidationResult> IsSaleValid(int userId, int carId, CancellationToken cancellationToken);
    }
}
