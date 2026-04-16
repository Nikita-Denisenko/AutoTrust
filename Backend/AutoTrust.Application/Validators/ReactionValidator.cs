using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class ReactionValidator : IReactionValidator
    {
        private readonly IRepository<Listing> _listingRepo;

        public ReactionValidator(IRepository<Listing> listingRepo)
        {
            _listingRepo = listingRepo;
        }

        public async Task<ValidationResult> CanCreateAsync(int currentUserId, CreateReactionDto dto, CancellationToken cancellationToken)
        {
            var listing = await _listingRepo
                .GetQuery()
                .FirstOrDefaultAsync(l => l.Id == dto.ListingId, cancellationToken);

            if (listing == null)
                return new ValidationResult(false, $"Listing with ID {dto.ListingId} does not exist.");

            bool hasUserReacted = listing.Reactions.Any(r => r.UserId == currentUserId);

            return hasUserReacted
                ? new ValidationResult(false, $"User with ID {currentUserId} already reacted to listing with ID {dto.ListingId}.")
                : new ValidationResult(true);
        }
    }
}
