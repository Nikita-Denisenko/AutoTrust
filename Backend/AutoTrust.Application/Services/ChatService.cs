using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;
using AutoTrust.Domain.Entities;
using AutoTrust.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace AutoTrust.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _repo;
        private readonly IRepository<User> _userRepo;
        private readonly IChatValidator _validator;
        private readonly IMapper _mapper;

        public ChatService
        (
            IRepository<Chat> repo, 
            IChatValidator validator, 
            IRepository<User> userRepo,
            IMapper mapper
        )
        {
            _repo = repo;
            _validator = validator;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<CreatedChatDto> CreateChatAsync(int currentUserId, CreateChatDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _validator.CanCreateAsync(dto, currentUserId, cancellationToken);

            if (!canCreate)
                throw new InvalidOperationException($"Failed to create Chat {error}");

            try
            {
                var chat = new Chat(currentUserId, dto.CompanionId);
                await _repo.AddAsync(chat, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                var companion = await _userRepo.GetQuery()
                    .AsNoTracking()
                    .FirstAsync(u => u.Id == dto.CompanionId, cancellationToken);

                var companionDto = _mapper.Map<UserShortDto>(companion);

                return new CreatedChatDto(chat.Id, companionDto);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create Chat: {ex.Message}");
            }
        }

        public async Task DeleteChatForAdminAsync(int chatId, CancellationToken cancellationToken)
        {
            var chat = await _repo.GetByIdAsync(chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} was not found to delete!");

            _repo.Delete(chat);
            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<AdminChatDto> GetChatForAdminAsync(int chatId, CancellationToken cancellationToken)
        {
            var chat = await _repo
                .GetQuery()
                .AsNoTracking()
                .Where(ch => ch.Id == chatId)
                .ProjectTo<AdminChatDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} was not found!");

            return chat;
        }

        public async Task<UserChatInfoDto> GetChatInfoAsync(int currentUserId, int chatId, CancellationToken cancellationToken)
        {
            var chat = await _repo
                .GetQuery()
                .AsNoTracking()
                .Include(ch => ch.ChatParticipants)
                .FirstOrDefaultAsync(u => u.Id == chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} was not found!");

            var companion = chat.ChatParticipants.First(cp => cp.UserId != currentUserId);

            var userChatInfo = new UserChatInfoDto
            (
                _mapper.Map<UserShortDto>(companion),
                _mapper.Map<MessageDto>(chat.PinnedMessage)
            );

            return userChatInfo;
        }

        public async Task<List<AdminChatDto>> GetChatsForAdminAsync(int userId, CancellationToken cancellationToken)
        {
            var chats = await _repo
                .GetQuery()
                .AsNoTracking()
                .Where(ch => ch.Messages.Count > 0 && ch.ChatParticipants.Any(cp => cp.UserId == userId))
                .OrderByDescending(ch => ch.Messages.OrderBy(m => m.SentAt).Last().SentAt)
                .ProjectTo<AdminChatDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return chats;
        }

        public async Task<List<UserChatListItemDto>> GetUserChatsAsync(int currentUserId, CancellationToken cancellationToken)
        {
            var chats = await _repo
                .GetQuery()
                .AsNoTracking()
                .Where(ch => ch.Messages.Count > 0 && ch.ChatParticipants.Any(cp => cp.UserId == currentUserId))
                .OrderByDescending(ch => ch.Messages.OrderBy(m => m.SentAt).Last().SentAt)
                .Select(ch => new UserChatListItemDto
                (
                    ch.Id,
                    _mapper.Map<UserShortDto>(ch.ChatParticipants.First(cp => cp.UserId != currentUserId)),
                    _mapper.Map<MessageDto>(ch.PinnedMessage))
                )
                .ToListAsync(cancellationToken);

            return chats;
        }

        public async Task PinMessageAsync(int currentUserId, int messageId, int chatId, CancellationToken cancellationToken)
        {
            var chat = await _repo
                .GetQuery()
                .Include(ch => ch.Messages)
                .Include(ch => ch.ChatParticipants)
                .Include(ch => ch.PinnedMessage)
                .FirstOrDefaultAsync(ch => ch.Id == chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} was not found!");

            if (!chat.ChatParticipants.Any(cp => cp.UserId == currentUserId))
                throw new InvalidOperationException($"Chat with ID {chatId} is not belongs to user with ID {currentUserId}");

            var message = chat.Messages.FirstOrDefault(m => m.Id == messageId);
            
            if (message == null)
                throw new InvalidOperationException($"Message with ID {messageId} is not belongs to chat with ID {chatId}");

            chat.PinMessage(message);

            await _repo.SaveChangesAsync(cancellationToken);
        }
        
        public async Task UnpinMessageAsync(int chatId, CancellationToken cancellationToken)
        {
            var chat = await _repo
                .GetQuery()
                .FirstOrDefaultAsync(ch => ch.Id == chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} was not found!");

            chat.UnpinCurrentMessage();
            
            await _repo.SaveChangesAsync(cancellationToken);
        }
    }
}
