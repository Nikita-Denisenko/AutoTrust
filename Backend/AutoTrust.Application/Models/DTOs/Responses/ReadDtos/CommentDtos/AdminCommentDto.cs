using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.CommentDtos
{
    public record AdminCommentDto
    {
        public int Id { get; init; }
        public UserShortDto User { get; init; }
        public ListingShortDto Listing { get; init; }
        public string Text { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsBlocked { get; init; }
        public bool IsDeleted { get; init; }
    };
}
