namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record UserShortDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string? AvatarUrl { get; init; }
    };
}
