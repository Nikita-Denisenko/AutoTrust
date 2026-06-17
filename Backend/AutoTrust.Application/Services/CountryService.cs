using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Country;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoTrust.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _repo;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cache;

        public CountryService(IRepository<Country> repo, IMapper mapper, IRedisCacheService cache)
        {
            _repo = repo;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<CountryDto>> GetCountriesAsync(CountryFilterDto filterDto, CancellationToken cancellationToken)
        {
            var page = filterDto.Page < 1 ? 1 : filterDto.Page;
            var size = filterDto.Size > 100 ? 100 : filterDto.Size;
            var searchText = string.IsNullOrWhiteSpace(filterDto.SearchText) ? "all" : filterDto.SearchText.Trim();
            var sortByAsc = filterDto.SortByAsc;

            var cacheKey = $"countries_{page}_{size}_{searchText}_{sortByAsc}";

            var cached = await _cache.GetAsync<List<CountryDto>>(cacheKey, cancellationToken);

            if (cached != null)
                return cached;

            var query = _repo.GetQuery().AsNoTracking();

            if (searchText != "all")
            {
                query = query.Where(c =>
                    c.RuName.Contains(searchText) ||
                    c.EnName.Contains(searchText) ||
                    c.Code.Contains(searchText));
            }

            query = filterDto.SortByAsc
                ? query.OrderBy(c => c.RuName)
                : query.OrderByDescending(c => c.RuName);

            query = query
                .Skip((page - 1) * size)
                .Take(size);

            var countries = await _mapper
                .ProjectTo<CountryDto>(query)
                .ToListAsync(cancellationToken);

            await _cache.SetAsync<List<CountryDto>>(cacheKey, countries, TimeSpan.FromHours(1), cancellationToken);

            return countries;
        }

        public async Task LoadCountriesAsync(string json, CancellationToken cancellationToken)
        {
            var countryDtos = JsonSerializer.Deserialize<List<CountryImportDto>>(json);

            if (countryDtos == null || !countryDtos.Any())
                throw new InvalidOperationException("No countries to load");

            foreach (var countryDto in countryDtos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                bool exists = await _repo.GetQuery()
                    .AnyAsync(c => c.Code == countryDto.Code, cancellationToken);

                if (exists)
                    continue;

                var flagUrl = Url.Create(countryDto.FlagImageUrl); 
                var country = new Country(
                    countryDto.EnName,   
                    countryDto.RuName,   
                    countryDto.Code,
                    flagUrl
                );

                await _repo.AddAsync(country, cancellationToken);
            }

            await _repo.SaveChangesAsync(cancellationToken);
        }

        private class CountryImportDto
        {
            [JsonPropertyName("flag")]
            public string FlagImageUrl { get; set; } = string.Empty;

            [JsonPropertyName("nameEn")]
            public string EnName { get; set; } = string.Empty;

            [JsonPropertyName("nameRu")]
            public string RuName { get; set; } = string.Empty;

            [JsonPropertyName("code")]
            public string Code { get; set; } = string.Empty;
        }
    }
}
