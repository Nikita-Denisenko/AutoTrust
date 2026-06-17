using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.City;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoTrust.Application.Services
{
    public class CityService : ICityService
    {
        private readonly IRepository<City> _repo;
        private readonly IMapper _mapper;
        private readonly IRepository<Country> _countryRepo;
        private readonly IRedisCacheService _cache;

        public CityService(IRepository<City> repo, IMapper mapper, IRepository<Country> countryRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _countryRepo = countryRepo;
        }

        public async Task<List<CityDto>> GetCitiesAsync(CityFilterDto? filterDto, CancellationToken cancellationToken = default)
        {
            var searchText = string.IsNullOrWhiteSpace(filterDto?.SearchText) ? "all" : filterDto.SearchText.Trim();
            var countryId = filterDto?.CountryId;
            var page = filterDto?.Page ?? 1;
            var size = filterDto?.Size ?? 20;
            var sortByAsc = filterDto?.SortByAsc ?? true;

            if (page < 1) page = 1;

            if (size < 1) size = 10;
            if (size > 100) size = 100;

            var countryIdStr = countryId?.ToString() ?? "all";
            var cacheKey = $"cities_{searchText}_{countryIdStr}_{page}_{size}_{sortByAsc}";

            var cached = await _cache.GetAsync<List<CityDto>>(cacheKey, cancellationToken);
            if (cached != null)
                return cached;

            var query = _repo.GetQuery().AsNoTracking();

            if (searchText != "all")
            {
                var search = searchText.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(search)
                );
            }

            if (countryId.HasValue)
                query = query.Where(c => c.CountryId == countryId.Value);

            query = sortByAsc
                ? query.OrderBy(c => c.Name)
                : query.OrderByDescending(c => c.Name);

   
            query = query
                .Skip((page - 1) * size)
                .Take(size);


            var cities = await _mapper
                .ProjectTo<CityDto>(query)
                .ToListAsync(cancellationToken);


            var ttl = cities.Any() ? TimeSpan.FromHours(1) : TimeSpan.FromMinutes(5);
            await _cache.SetAsync(cacheKey, cities, ttl, cancellationToken);

            return cities;
        }

        public async Task LoadCitiesAsync(string json, CancellationToken cancellationToken)
        {
            var cityDtos = JsonSerializer.Deserialize<List<CityImportDto>>(json);

            if (cityDtos == null || cityDtos.Count == 0)
                throw new InvalidOperationException("No cities to load");

            var russia = await _countryRepo.GetQuery()
                .FirstOrDefaultAsync(c => c.Code.ToLower() == "ru", cancellationToken);

            if (russia == null)
                throw new InvalidOperationException("Russia not found in database. Load countries first.");

            foreach (var cityDto in cityDtos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bool exists = await _repo.GetQuery()
                    .AnyAsync(c => c.Name == cityDto.Name && c.CountryId == russia.Id, cancellationToken);

                if (exists)
                    continue;

                var city = new City(cityDto.Name, russia.Id);
                await _repo.AddAsync(city, cancellationToken);
            }

            await _repo.SaveChangesAsync(cancellationToken);
        }

        private class CityImportDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;
        }
    }
}
