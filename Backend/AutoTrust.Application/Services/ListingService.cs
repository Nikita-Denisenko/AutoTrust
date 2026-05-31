using AutoMapper;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CityDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs.CountryDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ModelDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Enums;
using AutoTrust.Domain.Enums.OrderParams;
using Microsoft.EntityFrameworkCore;
using static AutoTrust.Domain.Enums.ListingType;

namespace AutoTrust.Application.Services
{
    public class ListingService : IListingService
    {
        private readonly IRepository<Listing> _repo;
        private readonly ISaleListingValidator _saleListingValidator;
        private readonly IBuyListingValidator _buyListingValidator;
        private readonly IMapper _mapper;

        public ListingService
        (
            IRepository<Listing> repo,
            ISaleListingValidator validator,
            IBuyListingValidator buyListingValidator,
            IMapper mapper
        )
        {
            _repo = repo;
            _saleListingValidator = validator;
            _buyListingValidator = buyListingValidator;
            _mapper = mapper;
        }

        public async Task ActivateListingAsync(int id, CancellationToken cancellationToken)
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

            var listing = _mapper.Map<Listing>(dto, opts => opts.Items["UserId"] = currentUserId);
            var buyDetails = _mapper.Map<BuyDetails>(dto);
            listing.SetBuyDetails(buyDetails);

            await _repo.AddAsync(listing, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreatedListingDto>(listing);
        }

        public async Task<CreatedListingDto> CreateSaleListingAsync(int currentUserId, CreateSaleListingDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _saleListingValidator.IsValidAsync(currentUserId, dto.CarId, cancellationToken);
            if (!canCreate)
                throw new InvalidOperationException($"Failed to create SaleListing: {error}");

            var listing = _mapper.Map<Listing>(dto, opts => opts.Items["UserId"] = currentUserId);
            var saleDetails = _mapper.Map<SaleDetails>(dto);
            listing.SetSaleDetails(saleDetails);

            await _repo.AddAsync(listing, cancellationToken);
            await _repo.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreatedListingDto>(listing);
        }

        public async Task DeactivateListingAsync(int id, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);
            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to deactivate!");
            listing.Deactivate();
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteListingAsync(int id, int currentUserId, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetQuery()
                .Include(l => l.SaleDetails)
                .Include(l => l.BuyDetails)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to delete");
            if (listing.UserId != currentUserId)
                throw new InvalidOperationException($"User cannot delete other users listings!");

            if (listing.Type == Sale)
                listing.SaleDetails.Delete();
            else listing.BuyDetails.Delete();

            listing.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }


        public async Task<List<FeedListingDto>> GetFeedListingsAsync(FeedListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();
            query = query.Where(l => !l.IsDeleted && l.IsActive);

            if (filterDto.Type != null)
                query = query.Where(l => l.Type == filterDto.Type);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);

            query = query.OrderByDescending(l => l.CreatedAt);
            query = query.Skip((filterDto.Page - 1) * filterDto.Size).Take(filterDto.Size);

            var result = await query.Select(l => new FeedListingDto
            {
                Id = l.Id,
                Name = l.Name,
                Author = new UserShortDto
                (
                    l.User.Id,
                    l.User.Name,
                    l.User.Surname,
                    l.User.AvatarUrl != null ? l.User.AvatarUrl.Value : null
                ),
                Type = l.Type,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                Location = new LocationDto
                (
                    new CityDto(l.CityId, l.City.CountryId, l.City.Name),
                    new CountryDto
                    (
                        l.City.Country.Id,
                        l.City.Country.RuName,
                        l.City.Country.EnName,
                        l.City.Country.Code,
                        l.City.Country.FlagImageUrl != null ? l.City.Country.FlagImageUrl.Value : null
                    )
                ),
                SaleInfoDto = l.SaleDetails != null ? new SaleInfoDto
                {
                    Price = l.SaleDetails.Price,
                    CarId = l.SaleDetails.CarId,
                    CarImageUrl = l.SaleDetails.Car.ImageUrl != null ? l.SaleDetails.Car.ImageUrl.Value : null,
                    ModelName = l.SaleDetails.Car.Model != null ? l.SaleDetails.Car.Model.Name : null,
                    CarColor = l.SaleDetails.Car.Color
                } : null,
                BuyInfoDto = l.BuyDetails != null ? new BuyInfoDto
                {
                    ModelId = l.BuyDetails.ModelId,
                    ModelName = l.BuyDetails.Model != null ? l.BuyDetails.Model.Name : null,
                    BrandImageUrl = l.BuyDetails.Model != null && l.BuyDetails.Model.Brand != null ? l.BuyDetails.Model.Brand.LogoUrl.Value : null,
                    MinPrice = l.BuyDetails.MinPrice,
                    MaxPrice = l.BuyDetails.MaxPrice,
                    CarColor = l.BuyDetails.CarColor
                } : null,
                Description = l.Description,
                ReactionsQuantity = l.Reactions.Count
            }).ToListAsync(cancellationToken);

            return result;
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
            if (!string.IsNullOrWhiteSpace(adminFilterDto.SearchText))
                query = query.Where(l => l.Name.ToLower().Contains(adminFilterDto.SearchText.ToLower()));

            query = adminFilterDto.SortByAsc
                ? query.OrderBy(l => l.CreatedAt)
                : query.OrderByDescending(l => l.CreatedAt);

            query = query.Skip((adminFilterDto.Page - 1) * adminFilterDto.Size).Take(adminFilterDto.Size);

            var result = await query.Select(l => new AdminListingDto
            {
                Id = l.Id,
                Name = l.Name,
                Author = new UserShortDto
                (
                    l.User.Id,
                    l.User.Name,
                    l.User.Surname,
                    l.User.AvatarUrl != null ? l.User.AvatarUrl.Value : null
                ),
                Type = l.Type,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                Location = new LocationDto
                (
                    new CityDto(l.CityId, l.City.CountryId, l.City.Name),
                    new CountryDto
                    (
                        l.City.Country.Id,
                        l.City.Country.RuName,
                        l.City.Country.EnName,
                        l.City.Country.Code,
                        l.City.Country.FlagImageUrl != null ? l.City.Country.FlagImageUrl.Value : null
                    )
                ),
                Description = l.Description,
                ReactionsQuantity = l.Reactions.Count,
                IsDeleted = l.IsDeleted,
                IsActive = l.IsActive
            }).ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<BuyListingDto>> GetUserBuyListingsAsync(int userId, BuyListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();
            query = query.Where(l => l.UserId == userId && !l.IsDeleted && l.Type == Buy);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);
            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
                query = query.Where(l => l.Name.ToLower().Contains(filterDto.SearchText.ToLower()));
            if (filterDto.ModelId != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.ModelId == filterDto.ModelId);
            if (filterDto.MinPrice != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.MinPrice == filterDto.MinPrice);
            if (filterDto.MaxPrice != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.MaxPrice == filterDto.MaxPrice);
            if (filterDto.MinReleaseYear != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.MinReleaseYear == filterDto.MinReleaseYear);
            if (filterDto.MaxReleaseYear != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.MaxReleaseYear == filterDto.MaxReleaseYear);
            if (filterDto.Color != null)
                query = query.Where(l => l.BuyDetails != null && l.BuyDetails.CarColor == filterDto.Color);

            query = filterDto.OrderParam switch
            {
                BuyListingOrderParam.ReleaseYear => filterDto.SortByAsc
                    ? query.OrderBy(l => l.BuyDetails != null ? l.BuyDetails.MaxReleaseYear : 0)
                    : query.OrderByDescending(l => l.BuyDetails != null ? l.BuyDetails.MaxReleaseYear : 0),
                BuyListingOrderParam.MaxPrice => filterDto.SortByAsc
                    ? query.OrderBy(l => l.BuyDetails != null ? l.BuyDetails.MaxPrice : 0)
                    : query.OrderByDescending(l => l.BuyDetails != null ? l.BuyDetails.MaxPrice : 0),
                _ => filterDto.SortByAsc
                    ? query.OrderBy(l => l.CreatedAt)
                    : query.OrderByDescending(l => l.CreatedAt)
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size).Take(filterDto.Size);

            var result = await query.Select(l => new BuyListingDto
            {
                Id = l.Id,
                Author = new UserShortDto
                (
                    l.User.Id,
                    l.User.Name,
                    l.User.Surname,
                    l.User.AvatarUrl != null ? l.User.AvatarUrl.Value : null
                ),
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                Location = new LocationDto
                (
                    new CityDto(l.CityId, l.City.CountryId, l.City.Name),
                    new CountryDto
                    (
                        l.City.Country.Id,
                        l.City.Country.RuName,
                        l.City.Country.EnName,
                        l.City.Country.Code,
                        l.City.Country.FlagImageUrl != null ? l.City.Country.FlagImageUrl.Value : null
                    )
                ),
                ModelId = l.BuyDetails != null ? l.BuyDetails.ModelId : 0,
                ModelName = l.BuyDetails != null && l.BuyDetails.Model != null ? l.BuyDetails.Model.Name : null,
                BrandImageUrl = l.BuyDetails != null && l.BuyDetails.Model != null && l.BuyDetails.Model.Brand != null ? l.BuyDetails.Model.Brand.LogoUrl.Value : null,
                MinPrice = l.BuyDetails != null ? l.BuyDetails.MinPrice : 0,
                MaxPrice = l.BuyDetails != null ? l.BuyDetails.MaxPrice : 0,
                MinReleaseYear = l.BuyDetails != null ? l.BuyDetails.MinReleaseYear : 0,
                MaxReleaseYear = l.BuyDetails != null ? l.BuyDetails.MaxReleaseYear : 0,
                CarColor = l.BuyDetails != null ? l.BuyDetails.CarColor : null,
                ReactionsQuantity = l.Reactions.Count,
                Description = l.Description,
                IsActive = l.IsActive
            }).ToListAsync(cancellationToken);

            return result;
        }

        public async Task<List<SaleListingDto>> GetUserSaleListingsAsync(int userId, SaleListingFilterDto filterDto, CancellationToken cancellationToken)
        {
            var query = _repo.GetQuery().AsNoTracking();
            query = query.Where(l => l.UserId == userId && !l.IsDeleted && l.Type == Sale);

            if (filterDto.CityId != null)
                query = query.Where(l => l.CityId == filterDto.CityId);
            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
                query = query.Where(l => l.Name.ToLower().Contains(filterDto.SearchText.ToLower()));
            if (filterDto.ModelId != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.ModelId == filterDto.ModelId);
            if (filterDto.MinPrice != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Price >= filterDto.MinPrice);
            if (filterDto.MaxPrice != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Price <= filterDto.MaxPrice);
            if (filterDto.MinReleaseYear != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.ReleaseYear >= filterDto.MinReleaseYear);
            if (filterDto.MaxReleaseYear != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.ReleaseYear <= filterDto.MaxReleaseYear);
            if (filterDto.Color != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.Color == filterDto.Color);
            if (filterDto.MinEngineMileage != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.EngineMileage >= filterDto.MinEngineMileage);
            if (filterDto.MaxEngineMileage != null)
                query = query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null && l.SaleDetails.Car.EngineMileage <= filterDto.MaxEngineMileage);

            query = filterDto.OrderParam switch
            {
                SaleListingOrderParam.ReleaseYear => filterDto.SortByAsc
                    ? query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderBy(l => l.SaleDetails.Car.ReleaseYear)
                    : query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderByDescending(l => l.SaleDetails.Car.ReleaseYear),
                SaleListingOrderParam.Price => filterDto.SortByAsc
                    ? query.Where(l => l.SaleDetails != null).OrderBy(l => l.SaleDetails.Price)
                    : query.Where(l => l.SaleDetails != null).OrderByDescending(l => l.SaleDetails.Price),
                SaleListingOrderParam.EngineMileage => filterDto.SortByAsc
                    ? query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderBy(l => l.SaleDetails.Car.EngineMileage)
                    : query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderByDescending(l => l.SaleDetails.Car.EngineMileage),
                SaleListingOrderParam.OwnersQuantity => filterDto.SortByAsc
                    ? query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderBy(l => l.SaleDetails.Car.OwnershipHistory.Count)
                    : query.Where(l => l.SaleDetails != null && l.SaleDetails.Car != null).OrderByDescending(l => l.SaleDetails.Car.OwnershipHistory.Count),
                _ => filterDto.SortByAsc
                    ? query.OrderBy(l => l.CreatedAt)
                    : query.OrderByDescending(l => l.CreatedAt)
            };

            query = query.Skip((filterDto.Page - 1) * filterDto.Size).Take(filterDto.Size);

            var result = await query.Select(l => new SaleListingDto
            {
                Id = l.Id,
                Author = new UserShortDto
                (
                    l.User.Id,
                    l.User.Name,
                    l.User.Surname,
                    l.User.AvatarUrl != null ? l.User.AvatarUrl.Value : null
                ),
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                Location = new LocationDto
                (
                    new CityDto(l.CityId, l.City.CountryId, l.City.Name),
                    new CountryDto
                    (
                        l.City.Country.Id,
                        l.City.Country.RuName,
                        l.City.Country.EnName,
                        l.City.Country.Code,
                        l.City.Country.FlagImageUrl != null ? l.City.Country.FlagImageUrl.Value : null
                    )
                ),
                Price = l.SaleDetails != null ? l.SaleDetails.Price : 0,
                Car = l.SaleDetails != null && l.SaleDetails.Car != null ? new PublicCarDto
                {
                    Id = l.SaleDetails.Car.Id,
                    Description = l.SaleDetails.Car.Description,
                    ReleaseYear = l.SaleDetails.Car.ReleaseYear,
                    ImageUrl = l.SaleDetails.Car.ImageUrl != null ? l.SaleDetails.Car.ImageUrl.Value : null,
                    Color = l.SaleDetails.Car.Color,
                    StateNumber = l.SaleDetails.Car.StateNumber != null ? l.SaleDetails.Car.StateNumber.Value : null,
                    EngineMileage = l.SaleDetails.Car.EngineMileage,
                    OwnershipsQuantity = l.SaleDetails.Car.OwnershipHistory.Count,
                    Model = new ModelShortDto
                    (
                        l.SaleDetails.Car.Model.Id,
                        l.SaleDetails.Car.Model.Name,
                        new BrandShortDto
                        (
                            l.SaleDetails.Car.Model.Brand.Id,
                            l.SaleDetails.Car.Model.Brand.Name,
                            l.SaleDetails.Car.Model.Brand.LogoUrl != null ? l.SaleDetails.Car.Model.Brand.LogoUrl.Value : null
                        )
                    ),
                    HasAccident = l.SaleDetails.Car.HasAccident,
                    InSale = l.SaleDetails.Car.InSale
                } : null,
                Description = l.Description,
                ReactionsQuantity = l.Reactions.Count,
                IsActive = l.IsActive
            }).ToListAsync(cancellationToken);

            return result;
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

            listing.BuyDetails!.UpdateInfo(dto.ModelId, dto.MinPrice, dto.MaxPrice, dto.MinReleaseYear, dto.MaxReleaseYear, dto.CarColor);
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateListingInfoAsync(int id, int currentUserId, UpdateListingInfoDto dto, CancellationToken cancellationToken)
        {
            var listing = await _repo.GetByIdAsync(id, cancellationToken);
            if (listing == null)
                throw new KeyNotFoundException($"Listing with ID {id} was not found to update!");
            if (listing.UserId != currentUserId)
                throw new InvalidOperationException("You cannot update other users Listings!");

            listing.UpdateInfo(dto.Name, dto.Description);
            await _repo.SaveChangesAsync(cancellationToken);
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

            listing.SaleDetails!.UpdateInfo(dto.CarId, dto.Price);
            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}