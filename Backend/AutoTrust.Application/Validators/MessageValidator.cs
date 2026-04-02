using AutoTrust.Application.Common;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoTrust.Application.Validators
{
    public class MessageValidator : IMessageValidator
    {
        public IRepository<User> _userRepo;
        public IRepository<Chat> _chatRepo;
        
        public MessageValidator(IRepository<User> userRepo, IRepository<Chat> chatRepo)
        {
            _userRepo = userRepo;
            _chatRepo = chatRepo;
        }

        public async Task<ValidationResult> CanCreateAsync(int currentUserId, CreateMessageDto dto, CancellationToken cancellationToken)
        {
            var sender = await _userRepo
                .GetQuery()
                .AsNoTracking()
                .Include(u => u.ChatParticipants)
                .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

            if (sender == null)
                return new ValidationResult(false, "Current User was not found!");

            if (!await _chatRepo.GetQuery().AsNoTracking().AnyAsync(ch => ch.Id == dto.ChatId, cancellationToken))
                return new ValidationResult(false, $"Chat with id {dto.ChatId} was not found!");

            if (!sender.ChatParticipants.Any(cp => cp.ChatId == dto.ChatId))
                return new ValidationResult(false, "Current User is not a participant of the chat!");

            return new ValidationResult(true);
        }
    }
}
