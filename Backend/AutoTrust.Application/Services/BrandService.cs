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
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AutoTrust.Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _repo;
        private readonly IRepository<Car> _carRepo;
        private readonly IBrandValidator _validator;

        public BrandService(IRepository<Brand> repo, IBrandValidator validator, IRepository<Car> carRepo)
        {
            _repo = repo;
            _validator = validator;
            _carRepo = carRepo;
        }

        public async Task<CreatedBrandDto> CreateBrandAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreate(createBrandDto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var brand = new Brand
                (
                    createBrandDto.Name,
                    createBrandDto.Description,
                    Url.Create(createBrandDto.LogoUrl),
                    createBrandDto.CountryId
                );

                await _repo.AddAsync(brand, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return new CreatedBrandDto
                (
                    brand.Id,
                    brand.Name,
                    brand.CountryId
                );
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create brand: {ex.Message}", ex);
            }
        }

        public async Task<PublicBrandDto> GetBrandAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await _repo.GetQuery().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
            
            if (brand == null)
                throw new KeyNotFoundException($"Brand with id {id} was not found!");

            return new PublicBrandDto
            (
                brand.Id,
                brand.Name,
                brand.Description,
                brand.LogoUrl.Value,
                _carRepo.GetQuery().Count(c => c.Model.BrandId == brand.Id),
                brand.Country.RuName
            );
        }

        public async Task<AdminBrandDto> GetBrandForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await _repo.GetQuery().FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            if (brand == null)
                throw new KeyNotFoundException($"Brand with id {id} was not found!");

            return new AdminBrandDto
            (
                brand.Id,
                brand.Name,
                brand.Description,
                brand.LogoUrl.Value,
                brand.Country.RuName,
                _carRepo.GetQuery().Count(c => c.Model.BrandId == brand.Id),
                brand.IsDeleted
            );
        }

        public async Task<List<PublicBrandListItemDto>> GetBrandsAsync(BrandFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery();

            query = query.Where(b => !b.IsDeleted);

            query = query.Where(b => b.Name.Contains(filterDto.SearchText));

            if (filterDto.CountryId.HasValue)
                query = query.Where(b => b.CountryId == filterDto.CountryId.Value);

            query = filterDto.OrderParam switch
            {
                BrandOrderParam.CarQuantity => filterDto.ByAscending 
                    ? query.OrderBy(b => _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id)) 
                    : query.OrderByDescending(b => _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id)),

                _ => filterDto.ByAscending
                    ? query.OrderBy(b => b.Name)
                    : query.OrderByDescending(b => b.Name),
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await query.Select(b => new PublicBrandListItemDto
            (
                b.Id,
                b.Name,
                b.LogoUrl.Value,
                _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id)
            )).ToListAsync(cancellationToken);
        }

        public async Task<List<AdminBrandListItemDto>> GetBrandsForAdminAsync(AdminBrandFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery();

            if (filterDto.IsDeleted != null) 
                query = filterDto.IsDeleted.Value 
                    ? query.Where(b => b.IsDeleted)
                    : query.Where(b => !b.IsDeleted);

            query = query.Where(b => b.Name.Contains(filterDto.SearchText));

            if (filterDto.CountryId.HasValue)
                query = query.Where(b => b.CountryId == filterDto.CountryId.Value);

            query = filterDto.OrderParam switch
            {
                BrandOrderParam.CarQuantity => filterDto.ByAscending
                    ? query.OrderBy(b => _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id))
                    : query.OrderByDescending(b => _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id)),

                _ => filterDto.ByAscending
                    ? query.OrderBy(b => b.Name)
                    : query.OrderByDescending(b => b.Name),
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

           return await query.Select(b => new AdminBrandListItemDto
           (
                b.Id,
                b.Name,
                b.LogoUrl.Value,
                _carRepo.GetQuery().Count(c => c.Model.BrandId == b.Id),
                b.IsDeleted
           )).ToListAsync(cancellationToken);
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
            catch(ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update brand: {ex.Message}", ex);
            }
        }
    }
}
