using AutoTrust.Application.Common;

namespace AutoTrust.Application.Interfaces.Validators
{
    public interface ILocationValidator
    {
        public Task<ValidationResult> CountryExistsAsync(int countryId, CancellationToken cancellationToken);
        public Task<ValidationResult> CityExistsAsync(int cityId, CancellationToken cancellationToken);
        public Task<ValidationResult> CityBelongsToCountryAsync(int cityId, int countryId, CancellationToken cancellationToken);
    }
}
