using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.City;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface ICityService
    {
        public Task<List<CityDto>> GetCitiesAsync(CityFilterDto filterDto, CancellationToken cancellationToken);
        public Task LoadCitiesAsync(string json, CancellationToken cancellationToken);
    }
}
