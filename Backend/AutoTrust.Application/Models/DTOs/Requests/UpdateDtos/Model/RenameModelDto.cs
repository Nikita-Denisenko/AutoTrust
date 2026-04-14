using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Model
{
    public record RenameModelDto
    (
        [MinLength(2)]
        [MaxLength(50)]
        string Name
    );
}
