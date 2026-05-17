using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.LocationDTOs;
using AutoTrust.Domain.Enums;

public class UserProfileDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Patronymic { get; set; }
    public DateOnly BirthDate { get; set; }
    public string? AvatarUrl { get; set; }
    public required string Gender { get; set; }
    public string? AboutInfo { get; set; }
    public LocationDto? Location { get; set; }
    public int ReviewsQuantity { get; set; }
    public int FollowersQuantity { get; set; }
    public int FollowingsQuantity { get; set; }
}