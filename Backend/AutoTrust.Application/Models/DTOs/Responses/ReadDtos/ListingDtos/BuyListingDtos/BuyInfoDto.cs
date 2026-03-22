using AutoTrust.Domain.Enums;

namespace AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ListingDtos.BuyListingDtos
{
    public record BuyInfoDto
    (
        int ModelId,
        string ModelName,
        string ModelImageUrl,
        decimal MinPrice,
        decimal MaxPrice,
        CarColor? CarColor
    );
}
