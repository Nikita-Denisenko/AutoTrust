using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class BuyDetails
    {
        public int Id { get; private set; }
        public int? ListingId { get; private set; }
        public Listing? Listing { get; private set; }
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
            int modelId,
            decimal minPrice, 
            decimal maxPrice, 
            int minReleaseYear, 
            int maxReleaseYear, 
            CarColor? carColor
        )
        {
            if (modelId <= 0)
                throw new ArgumentOutOfRangeException(nameof(modelId), modelId, "ModelId must be positive!");

            if (minPrice < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(minPrice),
                    minPrice,
                    "Minimal price cannot be negative!"
                    );

            if (maxPrice < minPrice)
                throw new ArgumentOutOfRangeException(
                   nameof(maxPrice),
                   maxPrice,
                   "Maximal price cannot be less than minimal price!"
                   );

            if (minReleaseYear < 1900 || minReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(
                    nameof(minReleaseYear),
                    minReleaseYear,
                    "Minimal release year cannot be less than 1900 or greater than the current year!"
                    );

            if (maxReleaseYear < minReleaseYear || maxReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(
                    nameof(maxReleaseYear),
                    maxReleaseYear,
                    "Maximal release year cannot be less than minimal release year or greater than the current year!"
                    );

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
                throw new ArgumentOutOfRangeException(
                    nameof(minPrice), 
                    minPrice, 
                    "Minimal price cannot be negative!"
                    );

            if (maxPrice < (minPrice ?? MaxPrice))
                throw new ArgumentOutOfRangeException(
                    nameof(maxPrice), 
                    maxPrice, 
                    "Maximal price cannot be less than minimal price!"
                    );

            if (minReleaseYear < 1900 || minReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(
                    nameof(minReleaseYear), 
                    minReleaseYear, 
                    "Minimal release year cannot be less than 1900 or greater than the current year!"
                    );

            if (maxReleaseYear < (minReleaseYear ?? MinReleaseYear) || maxReleaseYear > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(
                    nameof(maxReleaseYear),
                    maxReleaseYear,
                    "Maximal release year cannot be less than minimal release year or greater than the current year!"
                    );

            MinPrice = minPrice ?? MinPrice;
            MaxPrice = maxPrice ?? MaxPrice;
            MinReleaseYear = minReleaseYear ?? MinReleaseYear;
            MaxReleaseYear = maxReleaseYear ?? MaxReleaseYear;
            CarColor = carColor ?? CarColor;
        }
    }
}
