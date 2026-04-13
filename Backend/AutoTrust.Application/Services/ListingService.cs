using AutoMapper;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using static AutoTrust.Domain.Enums.ListingType;
using AutoTrust.Domain.Enums.OrderParams;

namespace AutoTrust.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IRepository<Listing> _repo;
        private readonly ISaleListingValidator _saleListingValidator;
        private readonly IBuyListingValidator _buyListingValidator;
        private readonly IMapper _mapper;
        private readonly IUserValidator _userValidator;

        public ListingService
        (
            IRepository<Listing> repo, 
            ISaleListingValidator validator,
            IBuyListingValidator buyListingValidator,
            IMapper mapper, 
            IUserValidator userValidator)
        {
            _repo = repo;
            _saleListingValidator = validator;
            _buyListingValidator = buyListingValidator;
            _mapper = mapper;
            _userValidator = userValidator;
        }

        public async Task ActivateAsync(int id, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to activate!");

            listing.Activate();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<CreatedListingDto> CreateBuyListingAsync(int currentUserId, CreateBuyListingDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _buyListingValidator.CanCreateAsync(dto, cancellationToken);

            if (!canCreate)
                throw new InvalidOperationException($"Failed to create BuyListing: {error}");

            try
            {
                var listing = _mapper.Map<Listing>(dto, opts => opts.Items["UserId"] = currentUserId);
                var buyDetails = _mapper.Map<BuyDetails>(dto);

                listing.SetBuyDetails(buyDetails);

                await _repo.AddAsync(listing, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedListingDto>(listing);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create BuyListing : {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create BuyListing : {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to create BuyListing : {ex.Message}", ex);
            }
        }

        public async Task<CreatedListingDto> CreateSaleListingAsync(int currentUserId, CreateSaleListingDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _saleListingValidator.IsValidAsync(currentUserId, dto.CarId, cancellationToken);

            if (!canCreate)
                throw new InvalidOperationException($"Failed to create SaleListing: {error}");

            try
            {
                var listing = _mapper.Map<Listing>(dto, opts => opts.Items["UserId"] = currentUserId);
                var saleDetails = _mapper.Map<SaleDetails>(dto);

                listing.SetSaleDetails(saleDetails);

                await _repo.AddAsync(listing, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedListingDto>(listing);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create SaleListing : {ex.Message}", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create SaleListing : {ex.Message}", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Failed to create SaleListing : {ex.Message}", ex);
            }
        }

        public async Task DeactivateAsync(int id, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to deactivate!");

            listing.Deactivate();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to delete");

            if (listing.UserId != currentUserId)
                throw new InvalidOperationException($"User cannot delete other users listings!");

            listing.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<FeedListingDto>> GetFeedListingsAsync(FeedListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            query = query.Where(l => !l.IsDeleted);

            if (filterDto.Type != null)
                query = query.Where(l => l.Type == filterDto.Type);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);

            query = query.Where(l => l.Name.ToLower().Contains(filterDto.SearchText.ToLower()));

            query = filterDto.SortByAsc 
                ? query.OrderBy(l => l.CreatedAt)
                : query.OrderByDescending(l => l.CreatedAt);

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);
            
            return await _mapper
                .ProjectTo<FeedListingDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AdminListingDto>> GetListingsForAdminAsync(AdminListingFilterDto adminFilterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            if (adminFilterDto.IsDeleted != null)
                query = query.Where(l => l.IsDeleted == adminFilterDto.IsDeleted.Value);

            if (adminFilterDto.Type != null)
                query = query.Where(l => l.Type == adminFilterDto.Type);

            if (adminFilterDto.CityId != null)
                query = query.Where(l => l.CityId == adminFilterDto.CityId);

            query = query.Where(l => l.Name.ToLower().Contains(adminFilterDto.SearchText.ToLower()));

            query = adminFilterDto.SortByAsc
               ? query.OrderBy(l => l.CreatedAt)
               : query.OrderByDescending(l => l.CreatedAt);

            query = query
                .Skip((adminFilterDto.Page - 1) * adminFilterDto.Size)
                .Take(adminFilterDto.Size);

            return await _mapper
                .ProjectTo<AdminListingDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<BuyListingDto>> GetUserBuyListingsAsync(int userId, BuyListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            query = query.Where(l => !l.IsDeleted);

            query = query.Where(l => l.Type == Buy);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);

            query = query.Where(l => l.Name.ToLower().Contains(filterDto.SearchText.ToLower()));

            if (filterDto.ModelId != null)
                query = query.Where(l => l.BuyDetails!.ModelId == filterDto.ModelId);

            if (filterDto.MinPrice != null)
                query = query.Where(l => l.BuyDetails!.MinPrice == filterDto.MinPrice);

            if (filterDto.MaxPrice != null)
                query = query.Where(l => l.BuyDetails!.MaxPrice == filterDto.MaxPrice);

            if (filterDto.MinReleaseYear != null)
                query = query.Where(l => l.BuyDetails!.MinReleaseYear == filterDto.MinReleaseYear);

            if (filterDto.MaxReleaseYear != null)
                query = query.Where(l => l.BuyDetails!.MaxReleaseYear == filterDto.MaxReleaseYear);

            if (filterDto.Color != null)
                query = query.Where(l => l.BuyDetails!.CarColor == filterDto.Color);

            query = filterDto.OrderParam switch
            {
                BuyListingOrderParam.ReleaseYear => filterDto.SortByAsc
                    ? query.OrderBy(l => l.BuyDetails!.MaxReleaseYear)
                    : query.OrderByDescending(l => l.BuyDetails!.MaxReleaseYear),

                BuyListingOrderParam.MaxPrice => filterDto.SortByAsc
                    ? query.OrderBy(l => l.BuyDetails!.MaxPrice)
                    : query.OrderByDescending(l => l.BuyDetails!.MaxPrice),

                _ => filterDto.SortByAsc
                    ? query.OrderBy(l => l.CreatedAt)
                    : query.OrderByDescending(l => l.CreatedAt)
            };

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await _mapper
                .ProjectTo<BuyListingDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SaleListingDto>> GetUserSaleListingsAsync(int userId, SaleListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();

            query = query.Where(l => !l.IsDeleted);

            query = query.Where(l => l.Type == Sale);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);

            query = query.Where(l => l.Name.ToLower().Contains(filterDto.SearchText.ToLower()));

            if (filterDto.ModelId != null)
                query = query.Where(l => l.SaleDetails!.Car.ModelId == filterDto.ModelId);

            if (filterDto.MinPrice != null)
                query = query.Where(l => l.SaleDetails!.Price >= filterDto.MinPrice);

            if (filterDto.MaxPrice != null)
                query = query.Where(l => l.SaleDetails!.Price <= filterDto.MaxPrice);

            if (filterDto.MinReleaseYear != null)
                query = query.Where(l => l.SaleDetails!.Car.ReleaseYear >= filterDto.MinReleaseYear);

            if (filterDto.MaxReleaseYear != null)
                query = query.Where(l => l.SaleDetails!.Car.ReleaseYear <= filterDto.MaxReleaseYear);

            if (filterDto.Color != null)
                query = query.Where(l => l.SaleDetails!.Car.Color == filterDto.Color);

            if (filterDto.MinEngineMileage != null)
                query = query.Where(l => l.SaleDetails!.Car.EngineMileage >= filterDto.MinEngineMileage);

            if (filterDto.MaxEngineMileage != null)
                query = query.Where(l => l.SaleDetails!.Car.EngineMileage <= filterDto.MaxEngineMileage);

            query = filterDto.OrderParam switch
            {
                SaleListingOrderParam.ReleaseYear => filterDto.SortByAsc
                    ? query.OrderBy(l => l.SaleDetails!.Car.ReleaseYear)
                    : query.OrderByDescending(l => l.SaleDetails!.Car.ReleaseYear),

                SaleListingOrderParam.Price => filterDto.SortByAsc
                    ? query.OrderBy(l => l.SaleDetails!.Price)
                    : query.OrderByDescending(l => l.SaleDetails!.Price),

                SaleListingOrderParam.EngineMileage => filterDto.SortByAsc
                    ? query.OrderBy(l => l.SaleDetails!.Car.EngineMileage)
                    : query.OrderByDescending(l => l.SaleDetails!.Car.EngineMileage),

                SaleListingOrderParam.OwnersQuantity => filterDto.SortByAsc
                    ? query.OrderBy(l => l.SaleDetails!.Car.OwnershipHistory.Count)
                    : query.OrderByDescending(l => l.SaleDetails!.Car.OwnershipHistory.Count),

                _ => filterDto.SortByAsc
                    ? query.OrderBy(l => l.CreatedAt)
                    : query.OrderByDescending(l => l.CreatedAt)
            };

            query = query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);

            return await _mapper
                .ProjectTo<SaleListingDto>(query)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateBuyListingAsync(int id, int currentUserId, UpdateBuyListingDto dto, CancellationToken cancellationToken)
        {
            var (canUpdate, error) = await _buyListingValidator.CanUpdateAsync(dto, cancellationToken);

            if (!canUpdate)
                throw new InvalidOperationException($"Failed to update BuyListing:{error}");

            var listing = await _repo.GetQuery()
                .Include(l => l.BuyDetails)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"BuyListing with ID {id} was not found to update!");

            if (listing.Type != Buy)
                throw new InvalidOperationException($"You cannot update this data in SaleListing!");

            if (listing.UserId != currentUserId)
                throw new InvalidOperationException("You cannot update other users BuyListings!");

            try
            {
                listing.BuyDetails!.UpdateInfo
                (
                    dto.ModelId,
                    dto.MinPrice,
                    dto.MaxPrice,
                    dto.MinReleaseYear,
                    dto.MaxReleaseYear,
                    dto.CarColor
                );

                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to update BuyListing: {ex.Message}", ex);
            }
        }

        public async Task UpdateListingInfoAsync(int id, int currentUserId, UpdateListingInfoDto dto, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to update!");

            if (listing.UserId != currentUserId)
                throw new InvalidOperationException("You cannot update other users Listings!");

            try
            {
                listing.UpdateInfo(dto.Name, dto.Description);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update Listing: {ex.Message}", ex);
            }
        }

        public async Task UpdateSaleListingAsync(int id, int currentUserId, UpdateSaleListingDto dto, CancellationToken cancellationToken)
        {
            if (dto.CarId != null)
            {
                var (isValid, error) = await _saleListingValidator.IsValidAsync(currentUserId, dto.CarId.Value, cancellationToken);

                if (!isValid)
                    throw new InvalidOperationException($"Failed to update SaleListing: {error}");
            }

            var listing = await _repo.GetQuery()
               .Include(l => l.SaleDetails)
               .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

            if (listing == null)
                throw new KeyNotFoundException($"SaleListing with ID {id} was not found to update!");

            if (listing.Type != Sale)
                throw new InvalidOperationException($"You cannot update this data in BuyListing!");

            if (listing.UserId != currentUserId)
                throw new InvalidOperationException("You cannot update other users SaleListings!");

            try
            {
                listing.SaleDetails!.UpdateInfo(dto.CarId, dto.Price);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to update BuyListing: {ex.Message}", ex);
            }
        }
    }
}
