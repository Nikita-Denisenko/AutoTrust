namespace AutoTrust.Domain.Entities
{
    public class CarOwnership
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public decimal MileageBefore { get; private set; }
        public decimal MileageAfter { get; private set; }
        public DateOnly FromDate { get; private set; }
        public DateOnly? ToDate { get; private set; }
        public int CarId { get; private set; }
        public Car Car { get; private set; }
        public bool HasAccident { get; private set; } = false;

        private CarOwnership() {}

        public CarOwnership
        (
            int userId,
            decimal mileageBefore,
            decimal mileageAfter,
            DateOnly fromDate,
            DateOnly toDate,
            int carId,
            bool hasAnccident
        )
        {
            UserId = userId;
            MileageBefore = mileageBefore;
            MileageAfter = mileageAfter;
            FromDate = fromDate;
            ToDate = toDate;
            CarId = carId;
            HasAccident = hasAnccident;
        }
    }
}
