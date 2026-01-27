using AutoTrust.Domain.Entities.Listings;
using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class BuyDetails
    {
        public int Id { get; private set; }
        public int ListingId { get; private set; }
        public Listing Listing { get; private set; }
        public int BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public decimal MinPrice { get; private set; }
        public decimal MaxPrice { get; private set; }   
        public int MinReleaseYear { get; private set; }
        public int MaxReleaseYear { get; private set; }
        public CarColor? CarColor { get; private set; }

        private BuyDetails() { }

        public BuyDetails
        (
            int listingId, 
            int brandId,
            decimal minPrice, 
            decimal maxPrice, 
            int minReleaseYear, 
            int maxReleaseYear, 
            CarColor? carColor
        )
        {
            ListingId = listingId;
            BrandId = brandId;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinReleaseYear = minReleaseYear;
            MaxReleaseYear = maxReleaseYear;
            CarColor = carColor;
        }
    }
}
