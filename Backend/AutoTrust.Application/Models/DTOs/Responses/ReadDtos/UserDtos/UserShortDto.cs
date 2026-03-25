namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.UserDtos
{
    public record UserShortDto
    (
       int Id,
       string Name,
       string Surname,
       string? AvatarUrl
    );
}
