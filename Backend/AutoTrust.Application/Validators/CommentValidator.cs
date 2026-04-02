using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class CommentValidator : ICommentValidator
    {
        private readonly IRepository<Listing> _repo;

        public CommentValidator(IRepository<Listing> repo) => _repo = repo;

        public async Task<ValidationResult> CanCreateAsync(CreateCommentDto dto, CancellationToken cancellationToken)
        {
            bool listingExists = await _repo.GetQuery().AnyAsync(l => l.Id == dto.ListingId, cancellationToken); 
            return new ValidationResult(listingExists, listingExists ? null : $"Listing with id {dto.ListingId} does not exists!");
        }
    }
}
