using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos
{
    public record ReviewDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Message { get; init; }
        public int Stars { get; init; }
        public UserShortDto Reviewer { get; init; }
        public DateTime CreatedAt { get; init; }
    };
}
