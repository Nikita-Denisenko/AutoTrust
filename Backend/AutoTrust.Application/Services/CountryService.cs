using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Country;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoTrust.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IRepository<Country> _repo;
        private readonly IMapper _mapper;

        public CountryService(IRepository<Country> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> GetCountriesAsync(CountryFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
            {
                query = query.Where(c =>
                    c.RuName.Contains(filterDto.SearchText) ||
                    c.EnName.Contains(filterDto.SearchText) ||
                    c.Code.Contains(filterDto.SearchText));
            }

            query = filterDto.SortByAsc
                ? query.OrderBy(c => c.RuName)
                : query.OrderByDescending(c => c.RuName);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await _mapper
                .ProjectTo<CountryDto>(query)
                .ToListAsync(cancellationToken);
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
