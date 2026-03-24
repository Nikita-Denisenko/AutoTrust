using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums.OrderParams;
using AutoTrust.Domain.Interfaces;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _repo;
        private readonly IBrandValidator _validator;
        private readonly IMapper _mapper;

        public BrandService
        (
            IRepository<Brand> repo, 
            IBrandValidator validator, 
            IMapper mapper
        )
        {
            _repo = repo;
            _validator = validator;
            _mapper = mapper;

        }

        private IQueryable<Brand> ApplyFilters(BrandFilterDto filterDto)
        {
            var query = _repo.GetQuery();

            if (filterDto is AdminBrandFilterDto adminFilterDto && adminFilterDto.IsActive != null)
                query = query.Where(b => b.IsActive == adminFilterDto.IsActive.Value);
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
            var validationResult = await _validator.CanCreate(createBrandDto, cancellationToken);

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
            var validationResult = await _validator.CanUpdate(id, updateBrandDto, cancellationToken);

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
                throw new InvalidOperationException($"Failed to create brand: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update brand: {ex.Message}", ex);
            }
        }
    }
}
