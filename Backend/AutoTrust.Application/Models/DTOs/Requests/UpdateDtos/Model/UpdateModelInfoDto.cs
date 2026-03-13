using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model
{
    public record UpdateModelInfoDto
    (
        [MinLength(2)]
        [MaxLength(50)]
        string? Name,

        [MinLength(1)]
        [MaxLength(900)]
        string? Description
    );
}