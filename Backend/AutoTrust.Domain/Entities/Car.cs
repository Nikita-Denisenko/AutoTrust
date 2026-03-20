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
            decimal engineMileage,
            int modelId,
            int locationCityId,
            bool hasAccident
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

            if (engineMileage < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(engineMileage), 
                    engineMileage, 
                    "Engine mileage cannot be negative!"
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
            EngineMileage = engineMileage;
            ModelId = modelId;
            LocationCityId = locationCityId;
            HasAccident = hasAccident;
            InSale = false;
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

        public void MakeForSale() => InSale = true;
        public void TakeOffSale() => InSale = false;
        public void Delete() => IsDeleted = true;
    }
}