namespace AutoTrust.Domain.Entities
{
    public class SaleDetails
    {
        public int Id { get; private set; }
        public int? ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public int CarId { get; private set; }
        public Car Car { get; private set; }
        public decimal Price { get; private set; }

        private SaleDetails() { }

        public SaleDetails
        (
            int carId,
            decimal price
        )
        {
            if (carId <= 0)
                throw new ArgumentOutOfRangeException(nameof(carId), carId, "CarId must be positive!");

            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price), price, "Price cannot be negative!");

            CarId = carId;
            Price = price;
        }

        public void UpdateInfo(int? carId, decimal? newPrice)
        {
            if (carId <= 0)
                throw new ArgumentOutOfRangeException(nameof(carId), carId, "CarId must be positive!");

            if (newPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(newPrice), newPrice, "Price cannot be negative!");

            CarId = carId ?? CarId;
            Price = newPrice ?? Price;
        }
    }
}
