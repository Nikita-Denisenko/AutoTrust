using AutoTrust.Domain.Enums;

public class FeedListingFilterDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 20;
    public int? CityId { get; set; }
    public ListingType? Type { get; set; }
}