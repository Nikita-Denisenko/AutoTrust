using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTrust.Application.Validators
{
    public class ReviewValidator : IReviewValidator
    {
        private readonly IUserValidator _userValidator;
        private readonly IRepository<Review> _reviewRepo;

        public ReviewValidator(IUserValidator userValidator, IRepository<Review> reviewRepo)
        {
            _userValidator = userValidator;
            _reviewRepo = reviewRepo;
        }

        public async Task<ValidationResult> CanCreateAsync(int currentUserId, CreateReviewDto dto, CancellationToken cancellationToken)
        {
            var (userExists, error) = await _userValidator.IsUserExistsAsync(dto.ReceiverId, cancellationToken);

            if (!userExists)
                return new ValidationResult(false, error);

            bool reviewExists = await _reviewRepo
                .GetQuery()
                .AnyAsync(r => r.ReviewerId == currentUserId && r.ReceiverId == dto.ReceiverId);

            return reviewExists
                ? new ValidationResult(false, $"User with ID {currentUserId} has already reviewed user with ID {dto.ReceiverId}.")
                : new ValidationResult(true);
        }
    }
}
