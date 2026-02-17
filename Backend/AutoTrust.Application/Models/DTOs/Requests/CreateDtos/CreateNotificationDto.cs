using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.CreateDtos
{
    public record CreateNotificationDto
    (
         [Required]
         [MinLength(1)]
         [MaxLength(30)]
         string Title,

         [Required]
         [MinLength(1)]
         [MaxLength(300)]
         string Text,
         
         [Required]
         [Range(1, int.MaxValue)]
         int UserId
    );
}
