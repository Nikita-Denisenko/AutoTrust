namespace AutoTrust.Domain.Entities
{
    public class CarOwnership
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public User User { get; private set; }
        public decimal MileageBefore { get; private set; }
        public decimal? MileageAfter { get; private set; } 
        public DateOnly FromDate { get; private set; }
        public DateOnly? ToDate { get; private set; }
        public bool HadMajorRepair { get; private set; }
        public int CarId { get; private set; }
        public Car Car { get; private set; }
        public bool IsCurrent { get; private set; }

        private CarOwnership() { }

        public CarOwnership
        (
            int userId,
            decimal mileageBefore,
            decimal? mileageAfter,
            DateOnly fromDate,
            DateOnly? toDate,
            bool hadMajorRepair,
            int carId
        )
        {
            if (userId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            if (carId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(carId), carId, "CarId must be positive");

            if (mileageBefore < 0) 
                throw new ArgumentOutOfRangeException(nameof(mileageBefore), mileageBefore, "Mileage cannot be negative");

            if (mileageAfter < 0)
                throw new ArgumentOutOfRangeException(nameof(mileageAfter), mileageAfter, "Mileage cannot be negative");

            UserId = userId;
            MileageBefore = mileageBefore;
            FromDate = fromDate;
            ToDate = toDate;
            HadMajorRepair = hadMajorRepair;
            CarId = carId;
            IsCurrent = toDate == null;
        }

        public void MakeMajorRepair() 
        {
            if (HadMajorRepair) 
                throw new InvalidOperationException("MajorRepair was already done!");

            HadMajorRepair = true;
        }

        public void EndOwnership(decimal mileageAfter)
        {
            if (!IsCurrent) 
                throw new InvalidOperationException("Only current owner can end ownership!");

            MileageAfter = mileageAfter;
            ToDate = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }
}