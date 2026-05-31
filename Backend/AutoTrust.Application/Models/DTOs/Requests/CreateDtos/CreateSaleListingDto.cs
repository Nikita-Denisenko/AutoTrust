using AutoTrust.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public class CreateSaleListingDto
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(4500)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CityId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CarId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}