using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing;
using AutoTrust.Domain.Entities;
using static AutoTrust.Domain.Enums.ListingType;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class SaleListingValidator : ISaleListingValidator
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Car> _carRepo;

        public SaleListingValidator(IRepository<User> userRepo, IRepository<Car> carRepo) 
        {
            _userRepo = userRepo;
            _carRepo = carRepo;
        }

        public async Task<ValidationResult> IsSaleValid (int userId, int carId, CancellationToken cancellationToken)
        {
            if(!await _carRepo
                .GetQuery()
                .AsNoTracking()
                .AnyAsync(c => c.Id == carId))
                return new ValidationResult(false, $"Car with ID {carId} does not exist!");

            var user = await _userRepo.GetQuery()
                .Include(u => u.Listings)
                .Include(u => u.CarOwnerships)
                .FirstAsync(u => u.Id == userId, cancellationToken);

            if (!user.CarOwnerships
                .Any(c => c.CarId == carId))
                return new ValidationResult(false, $"User with ID {userId} is not the owner of the car with {carId}!");

            if (user.Listings
                .Where(l => l.Type == Sale)
                .Any(l => l.SaleDetails!.CarId == carId))
                return new ValidationResult(false, $"User with ID {userId} already has an active sale listing for the car with ID {carId}!");

            return new ValidationResult(true);
        }
    }
}
