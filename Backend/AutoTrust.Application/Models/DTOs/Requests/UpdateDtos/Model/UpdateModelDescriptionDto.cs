using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model
{
    public record UpdateModelDescriptionDto
    (
        [MinLength(50)]
        [MaxLength(900)]
        string Description
    );
}
