using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record AdminReviewByUserDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public int Stars { get; init; }
        public UserShortDto Receiver { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool IsDeleted { get; init; }
    };
}
