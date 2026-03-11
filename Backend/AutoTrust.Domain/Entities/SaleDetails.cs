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
            if (listingId <= 0)
                throw new ArgumentException("ListingId must be positive!");

            if (carId <= 0)
                throw new ArgumentException("CarId must be positive!");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative!");

            ListingId = listingId;
            CarId = carId;
            Price = price;
        }

        public void UpdatePrice(decimal newPrice)
        {
            if (newPrice < 0)
                throw new ArgumentException("Price cannot be negative!");

            Price = newPrice;
        }
    }
}
