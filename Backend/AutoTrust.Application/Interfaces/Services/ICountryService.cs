using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Country;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface ICountryService
    {
        public Task<List<CountryDto>> GetCountriesAsync(CountryFilterDto filterDto, CancellationToken cancellationToken);
        public Task LoadCountriesAsync(string json, CancellationToken cancellationToken);
    }
}
