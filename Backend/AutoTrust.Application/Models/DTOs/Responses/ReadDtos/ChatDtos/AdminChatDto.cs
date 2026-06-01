using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.MessageDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ChatDtos
{
    public record AdminChatDto
    {
        public int Id { get; init; }
        public int LastMessageId { get; init; }
        public DateTime CreatedAt { get; init; }
        public MessageDto PinnedMessage { get; init; }
        public List<UserShortDto> Participants { get; init; }
    };
}
