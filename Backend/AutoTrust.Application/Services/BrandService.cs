using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums.OrderParams;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoTrust.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _repo;
        private readonly IRepository<Country> _countryRepo;
        private readonly IBrandValidator _validator;
        private readonly IMapper _mapper;

        public BrandService
        (
            IRepository<Brand> repo,
            IBrandValidator validator,
            IMapper mapper,
            IRepository<Country> countryRepo
        )
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;
            _countryRepo = countryRepo;
        }

        private IQueryable<Brand> ApplyFilters(BrandFilterDto filterDto)
        {
            var query = _repo.GetQuery()
                .AsNoTracking();

            if (filterDto is AdminBrandFilterDto adminFilterDto)
            {
                if (adminFilterDto.IsActive != null)
                    query = query.Where(b => b.IsActive == adminFilterDto.IsActive.Value);
            }
            else
                query = query.Where(b => b.IsActive);

            query = query.Where(b => b.Name.Contains(filterDto.SearchText));

            if (filterDto.CountryId.HasValue)
                query = query.Where(b => b.CountryId == filterDto.CountryId.Value);

            query = filterDto.OrderParam switch
            {
                BrandOrderParam.CarQuantity => filterDto.ByAscending
                    ? query.OrderBy(b => b.Models.SelectMany(m => m.Cars).Count())
                    : query.OrderByDescending(b => b.Models.SelectMany(m => m.Cars).Count()),

                _ => filterDto.ByAscending
                    ? query.OrderBy(b => b.Name)
                    : query.OrderByDescending(b => b.Name),
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);
            
            return query;
        }

        public async Task<CreatedBrandDto> CreateBrandAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreateAsync(createBrandDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var brand = _mapper.Map<Brand>(createBrandDto);

                await _repo.AddAsync(brand, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedBrandDto>(brand);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create brand: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create brand: {ex.Message}", ex);
            }
        }

        public async Task DeleteBrandAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.DeleteByIdAsync(id, cancellationToken);
            }
            catch(KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Brand by Id {id} was not found!");
            }
        }

        public async Task<PublicBrandDto> GetBrandAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await _repo.GetQuery()
                .AsNoTracking()
                .Where(b => b.Id == id)
                .ProjectTo<PublicBrandDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (brand == null)
                throw new KeyNotFoundException($"Brand with id {id} was not found!");

            return brand;
        }

        public async Task<AdminBrandDto> GetBrandForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await _repo.GetQuery()
                .AsNoTracking()
               .Where(b => b.Id == id)
               .ProjectTo<AdminBrandDto>(_mapper.ConfigurationProvider)
               .FirstOrDefaultAsync(cancellationToken);

            if (brand == null)
                throw new KeyNotFoundException($"Brand with id {id} was not found!");

            return brand;
        }

        public async Task<List<PublicBrandListItemDto>> GetBrandsAsync(BrandFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);

            return await _mapper
                .ProjectTo<PublicBrandListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminBrandListItemDto>> GetBrandsForAdminAsync(AdminBrandFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);

            return await _mapper
                .ProjectTo<AdminBrandListItemDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateBrandAsync(int id, UpdateBrandDto updateBrandDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanUpdateAsync(id, updateBrandDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            var brand = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Brand with id {id} was not found!");

            try
            {
                brand.Update
                (
                    updateBrandDto.Name, 
                    updateBrandDto.Description, 
                    updateBrandDto.LogoUrl != null 
                    ? Url.Create(updateBrandDto.LogoUrl)
                    : null
                );

                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to update brand: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update brand: {ex.Message}", ex);
            }
        }
        public async Task LoadBrandsAsync(string json, CancellationToken cancellationToken)
        {
            var brandDtos = JsonSerializer.Deserialize<List<BrandImportDto>>(json);

            if (brandDtos == null || !brandDtos.Any())
                throw new InvalidOperationException("No brands to load");

            foreach (var brandDto in brandDtos)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var country = await _countryRepo.GetQuery()
                    .FirstOrDefaultAsync(c => c.Code == brandDto.CountryCode, cancellationToken);

                if (country == null)
                {
                    Console.WriteLine($"Country with code {brandDto.CountryCode} not found. Skipping brand {brandDto.Name}");
                    continue;
                }

                bool exists = await _repo.GetQuery()
                    .AnyAsync(b => b.Name == brandDto.Name, cancellationToken);

                if (exists)
                    continue;

                var logoUrl = Url.Create(brandDto.LogoUrl);
                var brand = new Brand(brandDto.Name, brandDto.Description, logoUrl, country.Id);

                await _repo.AddAsync(brand, cancellationToken);
            }

            await _repo.SaveChangesAsync(cancellationToken);
        }

        private class BrandImportDto
        {
            [JsonPropertyName("name")]
            public string Name { get; set; } = string.Empty;

            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;

            [JsonPropertyName("logoUrl")]
            public string LogoUrl { get; set; } = string.Empty;

            [JsonPropertyName("countryCode")]
            public string CountryCode { get; set; } = string.Empty;
        }
    }
}
