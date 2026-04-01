using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateReactionDto
    (
         [Required]
         string Emoji,

         [Required]
         [MinLength(1)]
         [MaxLength(20)]
         string Name,

         [Required]
         [Range(1, int.MaxValue)]
         int ListingId
    );
}