using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class BuyDetails
    {
        public int Id { get; private set; }
        public int ListingId { get; private set; }
        public Listing Listing { get; private set; }
        public int ModelId { get; private set; }
        public Model Model { get; private set; }
        public decimal MinPrice { get; private set; }
        public decimal MaxPrice { get; private set; }   
        public int MinReleaseYear { get; private set; }
        public int MaxReleaseYear { get; private set; }
        public CarColor? CarColor { get; private set; }

        private BuyDetails() { }

        public BuyDetails
        (
            int listingId, 
            int modelId,
            decimal minPrice, 
            decimal maxPrice, 
            int minReleaseYear, 
            int maxReleaseYear, 
            CarColor? carColor
        )
        {
            if (listingId <= 0)
                throw new ArgumentException("ListingId must be positive!");

            if (modelId <= 0)
                throw new ArgumentException("ModelId must be positive!");

            if (minPrice < 0)
                throw new ArgumentException("Minimal price cannot be negative!");

            if (maxPrice < minPrice)
                throw new ArgumentException("Maximal price cannot be less than minimal price!");

            if (minReleaseYear < 1900)
                throw new ArgumentException("Minimal release year cannot be less than 1900!");

            if (maxReleaseYear < minReleaseYear)
                throw new ArgumentException("Maximal release year cannot be less than minimal release year!");

            ListingId = listingId;
            ModelId = modelId;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinReleaseYear = minReleaseYear;
            MaxReleaseYear = maxReleaseYear;
            CarColor = carColor;
        }

        public void UpdateInfo
        (
            decimal? minPrice, 
            decimal? maxPrice, 
            int? minReleaseYear, 
            int? maxReleaseYear,
            CarColor? carColor
        )
        {
            if (minPrice < 0)
                throw new ArgumentException("Minimal price cannot be negative!");

            if (maxPrice < (minPrice ?? MaxPrice))
                throw new ArgumentException("Maximal price cannot be less than minimal price!");

            if (minReleaseYear < 1900 || minReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentException("Minimal release year cannot be less than 1900 or greater than the current year!");

            if (maxReleaseYear < (minReleaseYear ?? MinReleaseYear) || maxReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentException("Maximal release year cannot be less than minimal release year or greater than the current year!");

            MinPrice = minPrice ?? MinPrice;
            MaxPrice = maxPrice ?? MaxPrice;
            MinReleaseYear = minReleaseYear ?? MinReleaseYear;
            MaxReleaseYear = maxReleaseYear ?? MaxReleaseYear;
            CarColor = carColor ?? CarColor;
        }
    }
}
