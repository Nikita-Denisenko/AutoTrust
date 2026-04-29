using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Brand;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Brand;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.BrandDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IBrandService
    {
        public Task<PublicBrandDto> GetBrandAsync(int id, CancellationToken cancellationToken);
        public Task<List<PublicBrandListItemDto>> GetBrandsAsync(BrandFilterDto filterDto, CancellationToken cancellationToken);
        public Task<AdminBrandDto> GetBrandForAdminAsync(int id, CancellationToken cancellationToken);
        public Task<List<AdminBrandListItemDto>> GetBrandsForAdminAsync(AdminBrandFilterDto filterDto, CancellationToken cancellationToken);
        public Task<CreatedBrandDto> CreateBrandAsync(CreateBrandDto createBrandDto, CancellationToken cancellationToken);
        public Task UpdateBrandAsync(int id, UpdateBrandDto updateBrandDto, CancellationToken cancellationToken);
        public Task DeleteBrandAsync(int id, CancellationToken cancellationToken);
        public Task LoadBrandsAsync(string json, CancellationToken cancellationToken);
    }
}
