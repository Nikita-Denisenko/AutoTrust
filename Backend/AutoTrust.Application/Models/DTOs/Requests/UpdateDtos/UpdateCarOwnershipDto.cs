using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos
{
    public record UpdateCarOwnershipDto
    (
        [Range(0, int.MaxValue)]
        decimal? MileageAfter,

        DateOnly? ToDate
    );
}
