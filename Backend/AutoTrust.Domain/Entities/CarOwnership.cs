using AutoTrust.Domain.ValueObjects;

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
        public Url BillOfSalePhotoUrl { get; private set; }
        public bool IsCurrent { get; private set; }

        private CarOwnership() { }

        public CarOwnership
        (
            int userId,
            decimal mileageBefore,
            DateOnly fromDate,
            bool hadMajorRepair,
            Url billOfSalePhotoUrl
        )
        {
            if (userId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(userId), userId, "UserId must be positive");

            if (mileageBefore < 0) 
                throw new ArgumentOutOfRangeException(nameof(mileageBefore), mileageBefore, "Mileage cannot be negative");

            UserId = userId;
            MileageBefore = mileageBefore;
            FromDate = fromDate;
            HadMajorRepair = hadMajorRepair;
            IsCurrent = true;
            BillOfSalePhotoUrl = billOfSalePhotoUrl;
        }

        public void MakeMajorRepair() 
        {
            if (HadMajorRepair) 
                throw new InvalidOperationException("MajorRepair was already done!");

            HadMajorRepair = true;
        }

        public void EndOwnership()
        {
            if (!IsCurrent) 
                throw new InvalidOperationException("Only current owner can end ownership!");

            MileageAfter = Car.EngineMileage;
            ToDate = DateOnly.FromDateTime(DateTime.UtcNow);
            IsCurrent = false;
        }
    }
}