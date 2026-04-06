using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IChatService
    {
        public Task<CreatedChatDto> CreateChatAsync(int currentUserId, CreateChatDto dto, CancellationToken cancellationToken);
        public Task DeleteChatForAdminAsync(int chatId, CancellationToken cancellationToken);
        public Task<AdminChatDto> GetChatForAdminAsync(int chatId, CancellationToken cancellationToken);
        public Task<List<AdminChatDto>> GetChatsForAdminAsync(int userId, CancellationToken cancellationToken);
        public Task<UserChatInfoDto> GetChatInfoAsync(int currentUserId, int chatId, CancellationToken cancellationToken);
        public Task<List<UserChatListItemDto>> GetUserChatsAsync(int currentUserId,  CancellationToken cancellationToken);
        public Task PinMessageAsync(int currentUserId, int messageId, int chatId, CancellationToken cancellationToken);
        public Task UnpinMessageAsync(int chatId, CancellationToken cancellationToken);
    }
}
