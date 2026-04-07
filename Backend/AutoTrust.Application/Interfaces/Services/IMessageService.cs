using AutoTrust.Application.Models.DTOs.Requests.Actions.Message;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Message;
using AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Message;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;

namespace AutoTrust.Application.Interfaces.Services
{
    public interface IMessageService
    {
        public Task<List<MessageDto>> GetMessagesAsync(int currentUserId, int chatId, MessageFilterDto filterDto, CancellationToken cancellationToken);
        public Task<List<AdminMessageDto>> GetMessagesForAdminAsync(AdminMessageFilterDto adminFilterDto, CancellationToken cancellationToken);
        public Task<CreatedMessageDto> CreateMessageAsync(int currentUserId, CreateMessageDto dto, CancellationToken cancellationToken);
        public Task UpdateMessageAsync(int Id, int currentUserId, UpdateMessageDto dto, CancellationToken cancellationToken);
        public Task MarkMessagesAsReadAsync(int currentUserId, int chatId, MarkAsReadMessagesDto dto, CancellationToken cancellationToken);
        public Task DeleteMessagesAsync(int currentUserId, DeleteMessagesDto dto, CancellationToken cancellationToken);
    }
}
