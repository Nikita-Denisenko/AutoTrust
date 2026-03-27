using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing
{
    public record UpdateListingInfoDto
    (
        [MaxLength(40)]
        string? Name,

        [MinLength(1)]
        [MaxLength(4500)]
        string? Description
    );
}
