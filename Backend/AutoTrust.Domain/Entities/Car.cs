using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class Car
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ReleaseYear { get; private set; }
        public string ImageUrl { get; private set; }
        public CarColor Color { get; private set; }
        public decimal EngineMileage { get; private set; }
        public List<CarOwnership> OwnershipHistory { get; private set; } = [];
        public int BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public int LocationCityId { get; private set; }
        public City LocationCity { get; private set; }
        public int LocationCountryId { get; private set; }
        public Country LocationCountry { get; private set; }

        private Car() { }

        public Car
        (
            string name,
            string description,
            string imageUrl,
            CarColor color,
            decimal engineMileage,
            int brandId,
            int locationCityId,
            int locationCountryId
        )
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Color = color;
            EngineMileage = engineMileage;
            BrandId = brandId;
            LocationCityId = locationCityId;
            LocationCountryId = locationCountryId;
        }
    }
}