using AutoTrust.Domain.Entities.Listings;

namespace AutoTrust.Domain.Entities
{
    public class SaleDetails
    {
        public int Id { get; private set; }
        public int ListingId { get; private set; }
        public Listing Listing { get; private set; }
        public int CarId { get; private set; }
        public Car Car { get; private set; }
        public decimal Price { get; private set; }

        private SaleDetails() { }

        public SaleDetails
        (
            int listingId,
            int carId,
            decimal price
        )
        {
            ListingId = listingId;
            CarId = carId;
            Price = price;
        }
    }
}
