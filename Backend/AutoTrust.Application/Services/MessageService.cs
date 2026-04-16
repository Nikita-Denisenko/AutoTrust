using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoTrust.Application.Interfaces.Repositories;
using AutoTrust.Application.Interfaces.Services;
using AutoTrust.Application.Interfaces.Validators;
using AutoTrust.Application.Models.DTOs.Requests.Actions.Message;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Message;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Message;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace AutoTrust.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageValidator _validator;
        private readonly IRepository<Message> _repo;
        private readonly IMapper _mapper;
        private readonly IRepository<Chat> _chatRepo;
        private readonly IUserValidator _userValidator;

        public MessageService
        (
            IMessageValidator validator, 
            IRepository<Message> repo, 
            IMapper mapper,
            IRepository<Chat> chatRepo,
            IUserValidator userValidator
        )
        {
            _validator = validator;
            _repo = repo;
            _mapper = mapper;
            _chatRepo = chatRepo;
            _userValidator = userValidator;
        }

        private IQueryable<Message> ApplyFilters(MessageFilterDto filterDto, IQueryable<Message> query)
        {
            if (filterDto is AdminMessageFilterDto adminDto)
            {
                if (adminDto.IsDeleted != null)
                    query = query.Where(m => m.IsDeleted == adminDto.IsDeleted.Value);

                if (adminDto.UserId != null)
                    query = query.Where(m => m.UserId == adminDto.UserId);

                if (adminDto.ChatId != null)
                    query = query.Where(m => m.ChatId == adminDto.ChatId);
            }
            else 
                query = query.Where(m => !m.IsDeleted);

            query = query.Where(m => m.Text.ToLower()
                .Contains(filterDto.SearchText.ToLower()));

            if (filterDto is AdminMessageFilterDto dto)
                query = dto.SortByAsc
                    ? query.OrderBy(m => m.SentAt)
                    : query.OrderByDescending(m => m.SentAt);
            else
                query = query.OrderByDescending(m => m.SentAt);

            return query
                .Skip((filterDto.Page - 1) * filterDto.Size)
                .Take(filterDto.Size);
        }

        public async Task<CreatedMessageDto> CreateMessageAsync(int currentUserId, CreateMessageDto dto, CancellationToken cancellationToken)
        {
            var (canCreate, error) = await _validator.CanCreateAsync(currentUserId, dto, cancellationToken);

            if (!canCreate)
                throw new InvalidOperationException($"Failed to create message: {error}");

            try
            {
                var message = _mapper.Map<Message>(dto, opts => opts.Items["UserId"] = currentUserId);

                await _repo.AddAsync(message, cancellationToken);
                await _repo.SaveChangesAsync(cancellationToken);

                return _mapper.Map<CreatedMessageDto>(message);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException($"Failed to create message:{ex.Message}", ex);
            }
            catch(ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to create message:{ex.Message}", ex);
            }
        }

        public async Task DeleteMessagesAsync(int currentUserId, DeleteMessagesDto dto, CancellationToken cancellationToken)
        {
            var messages = _repo
                .GetQuery()
                .Where(m => dto.MessageIds.Contains(m.Id));

            if (!await messages.AnyAsync(cancellationToken))
                throw new InvalidOperationException("Messages to delete were not found!");

            if (await messages.AnyAsync(m => m.UserId != currentUserId, cancellationToken))
                throw new InvalidOperationException("User cannot delete other user's messages!");

            foreach (var message in messages)
                message.Delete();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<MessageDto>> GetMessagesAsync
        (
            int currentUserId, 
            int chatId, 
            MessageFilterDto filterDto,
            CancellationToken cancellationToken
        )
        {
            var chat = await _chatRepo
                .GetQuery()
                .Include(ch => ch.ChatParticipants)
                .FirstOrDefaultAsync(ch => ch.Id == chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} does not exists!");

            if (!chat.ChatParticipants.Any(cp => cp.UserId == currentUserId))
                throw new InvalidOperationException($"Chat with ID {chatId} does not belongs to User with ID {currentUserId}");

            var query = _repo.GetQuery()
                .AsNoTracking()
                .Where(m => m.ChatId == chatId);

            if (!await query.AnyAsync(cancellationToken))
                return [];

            var messages = await ApplyFilters(filterDto, query)
                .ProjectTo<MessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return messages;
        }

        public async Task<List<AdminMessageDto>> GetMessagesForAdminAsync
        (
            AdminMessageFilterDto adminFilterDto, 
            CancellationToken cancellationToken
        )
        {
            if (adminFilterDto.ChatId != null)
            {
                bool chatExists = await _chatRepo
                    .GetQuery()
                    .AnyAsync(ch => ch.Id == adminFilterDto.ChatId, cancellationToken);

                if (!chatExists)
                    throw new KeyNotFoundException($"Chat with ID {adminFilterDto.ChatId} does not exists!");
            }

            if (adminFilterDto.UserId != null)
            {
                var (userExists, error) = await _userValidator.IsUserExistsAsync(adminFilterDto.UserId.Value, cancellationToken);

                if (!userExists)
                    throw new KeyNotFoundException($"User with ID {adminFilterDto.UserId} does not exists!");
            }

            if (adminFilterDto.ChatId != null && adminFilterDto.UserId != null)
            {
                bool chatBelongsToUser = await _chatRepo
                    .GetQuery()
                    .AnyAsync(ch => ch.Id == adminFilterDto.ChatId
                        && ch.ChatParticipants.Any(cp => cp.UserId == adminFilterDto.UserId), cancellationToken);

                if (!chatBelongsToUser)
                    throw new InvalidOperationException
                        ($"Chat with ID {adminFilterDto.ChatId} does not belongs to User with ID {adminFilterDto.UserId}");
            }

            var query = _repo.GetQuery()
              .AsNoTracking();

            if (!await query.AnyAsync(cancellationToken))
                return [];

            var messages = await ApplyFilters(adminFilterDto, query)
                .ProjectTo<AdminMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return messages;
        }

        public async Task MarkMessagesAsReadAsync
        (
            int currentUserId, 
            int chatId, 
            MarkAsReadMessagesDto dto, 
            CancellationToken cancellationToken
        )
        {
            var chat = await _chatRepo
               .GetQuery()
               .Include(ch => ch.ChatParticipants)
               .FirstOrDefaultAsync(ch => ch.Id == chatId, cancellationToken);

            if (chat == null)
                throw new KeyNotFoundException($"Chat with ID {chatId} does not exists!");

            bool chatBelongsToUser = chat.ChatParticipants.Any(cp => cp.UserId == currentUserId);

            if (!chatBelongsToUser)
                throw new InvalidOperationException
                    ($"Chat with ID {chatId} does not belongs to User with ID {currentUserId}");

            var messages = _repo
                .GetQuery()
                .Where(m => dto.MessageIds.Contains(m.Id));

            if (!await messages.AnyAsync(cancellationToken))
                throw new InvalidOperationException("Messages to mark as read were not found!");

            foreach (var message in messages)
                message.MarkAsRead();

            await _repo.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMessageAsync(int id, int currentUserId, UpdateMessageDto dto, CancellationToken cancellationToken)
        {
            var message = await _repo.GetByIdAsync(id, cancellationToken);

            if (message == null)
                throw new KeyNotFoundException($"Message with ID {id} was not found to update!");

            if (message.UserId != currentUserId)
                throw new InvalidOperationException($"User with ID {currentUserId} cannot update other user's messages!");

            try
            {
                message.Update(dto.Text);
                await _repo.SaveChangesAsync(cancellationToken);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException($"Failed to update Message: {ex.Message}", ex);
            }
        }
    }
}
