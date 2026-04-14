using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Listing;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IListingService
    {
        public Task<CreatedListingDto> CreateBuyListingAsync(int currentUserId, CreateBuyListingDto dto, CancellationToken cancellationToken);
        public Task<CreatedListingDto> CreateSaleListingAsync(int currentUserId, CreateSaleListingDto dto, CancellationToken cancellationToken);
        public Task<List<FeedListingDto>> GetFeedListingsAsync(FeedListingFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<BuyListingDto>> GetUserBuyListingsAsync(int userId, BuyListingFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<SaleListingDto>> GetUserSaleListingsAsync(int userId, SaleListingFilterDto filterDto,  CancellationToken cancellationToken);
        public Task<List<AdminListingDto>> GetListingsForAdminAsync(AdminListingFilterDto adminFilterDto, CancellationToken cancellationToken);
        public Task UpdateListingInfoAsync(int id, int currentUserId, UpdateListingInfoDto dto, CancellationToken cancellationToken);
        public Task UpdateBuyListingAsync(int id, int currentUserId, UpdateBuyListingDto dto, CancellationToken cancellationToken);
        public Task UpdateSaleListingAsync(int id, int currentUserId, UpdateSaleListingDto dto, CancellationToken cancellationToken);
        public Task DeleteListingAsync(int id, int currentUserId, CancellationToken cancellationToken);
        public Task DeactivateListingAsync(int id, CancellationToken cancellationToken);
        public Task ActivateListingAsync(int id, CancellationToken cancellationToken);
    }
}
