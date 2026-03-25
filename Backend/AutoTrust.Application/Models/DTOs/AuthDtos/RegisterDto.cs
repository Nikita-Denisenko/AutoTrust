using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.AuthDtos
{
    public record RegisterDto
    (
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        string Name,

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        string Surname,

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        string Patronymic,

        DateOnly BirthDate,

        Gender Gender,

        [Required]
        [Range(1, int.MaxValue)]
        int CityId,

        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [Phone]
        string Phone,

        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        string Password
    );  
}
