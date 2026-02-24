using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class Car
    {
        public int Id { get; private set; } 
        public string Description { get; private set; }
        public int ReleaseYear { get; private set; }
        public string ImageUrl { get; private set; }
        public CarColor Color { get; private set; }
        public string StateNumber { get; private set; }
        public decimal EngineMileage { get; private set; }
        public List<CarOwnership> OwnershipHistory { get; private set; } = [];
        public int ModelId { get; private set; }
        public Model Model { get; private set; }
        public int LocationCityId { get; private set; }
        public City LocationCity { get; private set; }
        public int LocationCountryId { get; private set; }
        public Country LocationCountry { get; private set; }
        public bool HasAccident { get; private set; } = false;
        public bool InSale { get; private set; } = false;
        public bool IsDeleted { get; private set; } = false;

        private Car() { }

        public Car
        (
            string description,
            int releaseYear,
            string imageUrl,
            CarColor color,
            string stateNumber,
            decimal engineMileage,
            int modelId,
            int locationCityId,
            int locationCountryId,
            bool hasAccident
        )
        {
            Description = description;
            ReleaseYear = releaseYear;
            ImageUrl = imageUrl;
            Color = color;
            StateNumber = stateNumber;
            EngineMileage = engineMileage;
            ModelId = modelId;
            LocationCityId = locationCityId;
            LocationCountryId = locationCountryId;
            HasAccident = hasAccident;
            InSale = false;
        }
    }
}