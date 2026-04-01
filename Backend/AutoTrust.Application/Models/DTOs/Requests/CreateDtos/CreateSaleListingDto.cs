using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateSaleListingDto
    (
        [Required]
        [MaxLength(40)]
        string Name,

        [Required]
        [MinLength(1)]
        [MaxLength(4500)]
        string Description,

        [Required]
        [Range(1, int.MaxValue)]
        int CityId,
        
        [Required]
        [Range(1, int.MaxValue)]
        int CarId,
        
        [Required]
        [Range(0, int.MaxValue)]
        decimal Price
    );
}
