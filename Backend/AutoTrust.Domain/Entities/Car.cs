using AutoTrust.Domain.Enums;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class Car
    {
        public int Id { get; private set; } 
        public string Description { get; private set; }
        public int ReleaseYear { get; private set; }
        public Url ImageUrl { get; private set; }
        public CarColor Color { get; private set; }
        public StateNumber StateNumber { get; private set; }
        public decimal EngineMileage { get; private set; }
        public ICollection<CarOwnership> OwnershipHistory { get; private set; } = [];
        public int ModelId { get; private set; }
        public Model Model { get; private set; }
        public int LocationCityId { get; private set; }
        public City LocationCity { get; private set; }
        public bool HasAccident { get; private set; } = false;
        public bool InSale { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;

        private Car() { }

        public Car
        (
            string description,
            int releaseYear,
            Url imageUrl,
            CarColor color,
            StateNumber stateNumber,
            int modelId,
            int locationCityId,
            bool hasAccident,
            int userId,
            DateOnly fromDate,
            bool hadMajorRepair,
            Url billOfSalePhotoUrl
        )
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (releaseYear < 1900 || releaseYear > DateTime.UtcNow.Year)
                throw new ArgumentOutOfRangeException(
                    nameof(releaseYear), 
                    releaseYear, 
                    "Year cannot be less than 1900 or greater than the current year!"
                    );

            if (modelId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(modelId), 
                    modelId, 
                    "ModelId must be positive!"
                    );

            if (locationCityId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(locationCityId), 
                    locationCityId, 
                    "LocationCityId must be positive!"
                    );


            Description = description;
            ReleaseYear = releaseYear;
            ImageUrl = imageUrl;
            Color = color;
            StateNumber = stateNumber;
            EngineMileage = 0;
            ModelId = modelId;
            LocationCityId = locationCityId;
            HasAccident = hasAccident;
            InSale = false;

            var currentOwnership = new CarOwnership
            (
                userId,
                0,
                fromDate,
                hadMajorRepair,
                Id,
                billOfSalePhotoUrl
            );

            OwnershipHistory.Add(currentOwnership);
        }

        public void UpdateInfo
        (
            string? description,
            Url? imageUrl,
            CarColor? color,
            StateNumber? stateNumber,
            decimal? engineMileage,
            bool? hasAccident
        ) 
        {
            if (description != null && string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (engineMileage < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(engineMileage), 
                    engineMileage, 
                    "Engine mileage cannot be negative!"
                    );

            Description = description ?? Description;
            ImageUrl= imageUrl ?? ImageUrl;
            Color = color ?? Color;
            StateNumber = stateNumber ?? StateNumber;
            EngineMileage = engineMileage ?? EngineMileage;  
            HasAccident = hasAccident ?? HasAccident;
        }

        public void TransferOwnership(int newOwnerId, Url billOfSalePhotoUrl)
        {
            if (newOwnerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(newOwnerId), newOwnerId, "New owner id must be positive!");

            if (!InSale)
                throw new InvalidOperationException("Car is not for sale");

            var currentOwnership = OwnershipHistory.First(o => o.IsCurrent);

            if (newOwnerId == currentOwnership.UserId)
                throw new InvalidOperationException("Cannot transfer ownership to yourself");

            try
            {
                currentOwnership.EndOwnership();
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

            var newOwnership = new CarOwnership
            (
                newOwnerId,
                currentOwnership.MileageAfter!.Value,
                DateOnly.FromDateTime(DateTime.UtcNow),
                currentOwnership.HadMajorRepair,
                Id,
                billOfSalePhotoUrl
            );

            OwnershipHistory.Add(newOwnership);
        }

        public void MakeForSale() 
        {
            if (InSale)
                throw new InvalidOperationException("Car is already for sale!");

            InSale = true;
        }
        public void TakeOffSale()
        {
            if (!InSale)
                throw new InvalidOperationException("Car is not for sale!");

            InSale = false;
        }
        public void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Car is already deleted");

            IsDeleted = true;
        }
    }
}