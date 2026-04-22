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

namespace AutoTrust.Application.Services
{
    public class CityService : ICityService
    {
        private readonly IRepository<City> _repo;
        private readonly IMapper _mapper;
        private readonly IRepository<Country> _countryRepo;

        public CityService(IRepository<City> repo, IMapper mapper, IRepository<Country> countryRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _countryRepo = countryRepo;
        }

        public async Task<List<CityDto>> GetCitiesAsync(CityFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
                query = query.Where(c => c.Name.Contains(filterDto.SearchText));

            if (filterDto.CountryId.HasValue)
                query = query.Where(c => c.CountryId == filterDto.CountryId.Value);

            query = filterDto.SortByAsc
                ? query.OrderBy(c => c.Name)
                : query.OrderByDescending(c => c.Name);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await _mapper
                .ProjectTo<CityDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task LoadCitiesAsync(string json, CancellationToken cancellationToken)
        {
            var cityDtos = JsonSerializer.Deserialize<List<CityImportDto>>(json);

            if (cityDtos == null || !cityDtos.Any())
                throw new InvalidOperationException("No cities to load");

            var russia = await _countryRepo.GetQuery()
                .FirstOrDefaultAsync(c => c.EnName.ToUpper() == "RUSSIA", cancellationToken);

            if (russia == null)
                throw new InvalidOperationException("Russia not found in database. Load countries first.");

            foreach (var cityDto in cityDtos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (cityDto.CountryId != russia.Id)
                    throw new InvalidOperationException("You can load only russian cities");

                var city = new City(cityDto.Name, cityDto.CountryId);

                await _repo.AddAsync(city, cancellationToken);
            }

            await _repo.SaveChangesAsync(cancellationToken);
        }

        private class CityImportDto
        {
            public string Name { get; set; } = string.Empty;
            public int CountryId { get; set; }
        }
    }
}