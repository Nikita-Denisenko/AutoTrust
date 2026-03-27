using System.ComponentModel.DataAnnotations;

namespace AutoTrust.Application.Models.DTOs.Requests.UpdateDtos.Listing
{
    public record UpdateSaleListingDto
    (
        [Range(1, int.MaxValue)]
        int? CarId,

        [Range(0, int.MaxValue)]
        decimal? Price
    );
}
