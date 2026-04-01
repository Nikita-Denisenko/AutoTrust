using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Car;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Car;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CarDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface ICarService
    {
        public Task<PublicCarDto> GetCarAsync(int id, CancellationToken cancellationToken);
        public Task<List<PublicCarListItemDto>> GetCarsAsync(CarFilterDto filterDto, CancellationToken cancellationToken);
        public Task<AdminCarDto> GetCarForAdminAsync(int id, CancellationToken cancellationToken);
        public Task<List<AdminCarListItemDto>> GetCarsForAdminAsync(AdminCarFilterDto filterDto, CancellationToken cancellationToken);
        public Task<CreatedCarDto> CreateCarAsync(int currentUserId, CreateCarDto createCarDto, CancellationToken cancellationToken);
        public Task UpdateCarAsync(int id, UpdateCarDto updateCarDto, CancellationToken cancellationToken);
        public Task DeleteCarAsync(int id, CancellationToken cancellationToken);
    }
}

