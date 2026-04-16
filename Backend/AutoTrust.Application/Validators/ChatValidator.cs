using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class ChatValidator : IChatValidator
    {
        private readonly IRepository<Chat> _repo;
        private readonly IRepository<User> _userRepo;

        public ChatValidator(IRepository<Chat> repo, IRepository<User> userRepo)
        {
            _repo = repo;
            _userRepo = userRepo;
        }

        public async Task<ValidationResult> CanCreateAsync(CreateChatDto dto, int currentUserId, CancellationToken cancellationToken)
        {
            if (!await _userRepo.GetQuery().AsNoTracking().AnyAsync(u => u.Id == dto.CompanionId, cancellationToken))
                return new ValidationResult(false, $"Companion with ID {dto.CompanionId} does not exists!");

            if (dto.CompanionId == currentUserId)
                return new ValidationResult(false, "User cannot create a chat with himself!");

            bool hasChatWithCompanion = await _repo
                .GetQuery()
                .AsNoTracking()
                .Include(ch => ch.ChatParticipants)
                .AnyAsync(ch => ch.ChatParticipants
                    .Any(cp => cp.Id == currentUserId)
                    && ch.ChatParticipants
                    .Any(cp => cp.Id == dto.CompanionId), cancellationToken);

            return hasChatWithCompanion 
                ? new ValidationResult(false, "User already has chat with this companion!") 
                : new ValidationResult(true);
        }
    }
}
