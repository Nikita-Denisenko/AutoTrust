using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.Actions.Car
{
    public record TransferOwnershipDto
    (
        [Required]
        [Range(1, int.MaxValue)]
        int NewOwnerId,

        [Required]
        [Url]
        string BillOfSalePhotoUrl
    );
}
