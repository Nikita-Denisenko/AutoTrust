using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Domain.Entities;
using static AutoTrust.Domain.Enums.ListingType;
using Microsoft.EntityFrameworkCore;
using AutoTrust.Application.Interfaces.Repositories;

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

        public async Task<ValidationResult> IsValidAsync(int userId, int carId, CancellationToken cancellationToken)
        {
            if (!await _carRepo.GetQuery().AnyAsync(c => c.Id == carId, cancellationToken))
                return new ValidationResult(false, $"Car with ID {carId} does not exist!");

            var user = await _userRepo.GetQuery()
                .Include(u => u.Listings)
                .Include(u => u.CarOwnerships)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null)
                return new ValidationResult(false, $"User with ID {userId} does not exist!");

            if (!user.CarOwnerships.Any(c => c.CarId == carId))
                return new ValidationResult(false, $"User is not the owner of car {carId}!");

            if (user.Listings
                .Where(l => !l.IsDeleted && l.Type == Sale && l.SaleDetails != null && !l.SaleDetails.IsDeleted)
                .Any(l => l.SaleDetails.CarId == carId))
                return new ValidationResult(false, $"Car {carId} is already on sale!");


            return new ValidationResult(true);
        }
    }
}
