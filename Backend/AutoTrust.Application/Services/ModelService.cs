using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Services
{
    public class ModelService : IModelService
    {
        private readonly IRepository<Model> _repo;
        private readonly IModelValidator _validator;
        private readonly IRepository<Brand> _brandRepo;
        private readonly IMapper _mapper;

        public ModelService
        (
            IRepository<Model> repo,
            IModelValidator validator,
            IRepository<Brand> brandRepo,
            IMapper mapper
        )
        {
            _repo = repo;
            _validator = validator;
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        private IQueryable<Model> ApplyFilters(ModelFilterDto filterDto)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (filterDto is AdminModelFilterDto adminDto && adminDto.IsActive != null)
                query = query.Where(m => m.IsActive == adminDto.IsActive.Value);
            else
                query = query.Where(m => m.IsActive);

            if (filterDto.BrandId.HasValue)
                query = query.Where(m => m.BrandId == filterDto.BrandId.Value);

            query = query.Where(m => m.Name.ToLower().Contains(filterDto.SearchText.ToLower()));

            query = filterDto.SortByAsc
                ? query.OrderBy(m => m.Name)
                : query.OrderByDescending(m => m.Name);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return query;
        }

        public async Task<CreatedModelDto> CreateModelAsync(CreateModelDto dto, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.CanCreateAsync(dto, cancellationToken);

            if (!validationResult.IsValid)
                throw new InvalidOperationException(validationResult.ErrorMessage);

            try
            {
                var model = _mapper.Map<Model>(dto);

                await _repo.AddAsync(model, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedModelDto>(model);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create model: {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create model: {ex.Message}", ex);
            }
        }

        public async Task DeleteModelAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repo.GetByIdAsync(id, cancellationToken);

            if (model == null)
                throw new KeyNotFoundException($"Model with ID {id} was not found!");

            model.Deactivate();
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<ModelDto> GetModelAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repo.GetQuery()
                .AsNoTracking()
                .Where(m => m.Id == id)
                .ProjectTo<ModelDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
                throw new KeyNotFoundException($"Model with ID {id} was not found!");

            return model;
        }

        public async Task<AdminModelDto> GetModelForAdminAsync(int id, CancellationToken cancellationToken)
        {
            var model = await _repo.GetQuery()
                .AsNoTracking()
                .Where(m => m.Id == id)
                .ProjectTo<AdminModelDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (model == null)
                throw new KeyNotFoundException($"Model with ID {id} was not found!");

            return model;
        }

        public async Task<List<ModelListItemDto>> GetModelsAsync(ModelFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);
            return await _mapper.ProjectTo<ModelListItemDto>(query).ToListAsync(cancellationToken);
        }

        public async Task<List<AdminModelListItemDto>> GetModelsForAdminAsync(AdminModelFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = ApplyFilters(filterDto);
            return await _mapper.ProjectTo<AdminModelListItemDto>(query).ToListAsync(cancellationToken);
        }

        public async Task RenameModelAsync(int id, RenameModelDto dto, CancellationToken cancellationToken)
        {
            var model = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Model with ID {id} was not found!");

            try
            {
                model.Rename(dto.Name);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to rename model: {ex.Message}", ex);
            }
        }

        public async Task UpdateModelDescriptionAsync(int id, UpdateModelDescriptionDto dto, CancellationToken cancellationToken)
        {
            var model = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Model with ID {id} was not found!");

            try
            {
                model.UpdateDescription(dto.Description);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update description: {ex.Message}", ex);
            }
        }

        public async Task UpdateModelImageAsync(int id, UpdateModelImageDto dto, CancellationToken cancellationToken)
        {
            var model = await _repo.GetByIdAsync(id, cancellationToken)
                ?? throw new KeyNotFoundException($"Model with ID {id} was not found!");

            try
            {
                model.UpdateImage(Url.Create(dto.ImageUrl));
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update image: {ex.Message}", ex);
            }
        }
    }
}