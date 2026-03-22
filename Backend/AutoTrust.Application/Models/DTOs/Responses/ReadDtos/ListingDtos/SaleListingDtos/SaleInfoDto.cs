using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.SaleListingDtos
{
    public record SaleInfoDto
    (
        decimal Price,
        int CarId,
        string CarImageUrl,
        string ModelName,
        CarColor CarColor
    );
}