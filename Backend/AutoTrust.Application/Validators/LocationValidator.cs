using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoTrust.Application.Validators
{
    public class LocationValidator : ILocationValidator
    {
        private readonly IRepository<Country> _countryRepo;
        private readonly IRepository<City> _cityRepo;

        public LocationValidator(IRepository<Country> countryRepo, IRepository<City> cityRepo)
        {
            _countryRepo = countryRepo;
            _cityRepo = cityRepo;
        }

        public async Task<ValidationResult> CountryExistsAsync(int countryId, CancellationToken cancellationToken)
        {
            bool exists = await _countryRepo.GetQuery().AnyAsync(c => c.Id == countryId, cancellationToken);
            return new ValidationResult(exists, exists ? null : $"Country with ID {countryId} does not exist!");
        }

        public async Task<ValidationResult> CityExistsAsync(int cityId, CancellationToken cancellationToken)
        {
            bool exists = await _cityRepo.GetQuery().AnyAsync(c => c.Id == cityId, cancellationToken);
            return new ValidationResult(exists, exists ? null : $"City with ID {cityId} does not exist!");
        }

        public async Task<ValidationResult> CityBelongsToCountryAsync(int cityId, int countryId, CancellationToken cancellationToken)
        {
            bool belongs = await _cityRepo.GetQuery()
                .AnyAsync(c => c.Id == cityId && c.CountryId == countryId, cancellationToken);
            return new ValidationResult(belongs, belongs ? null : $"City with ID {cityId} does not belong to Country with ID {countryId}!");
        }
    }
}
