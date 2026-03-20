using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class Model
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Url ImageUrl { get; private set; }
        public int BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public bool IsActive { get; private set; } = true;
        public ICollection<Car> Cars { get; private set; } = [];

        private Model() { }

        public Model
        (
            string name, 
            string description,
            Url imageUrl,
            int brandId 
        )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty!");

            if (brandId <= 0)
                throw new ArgumentOutOfRangeException(nameof(brandId), brandId, "BrandId must be positive!");

            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            BrandId = brandId;
        }

        public void Rename(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Name cannot be empty!");

            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("Description cannot be empty!");

            Description = newDescription;
        }


        public void UpdateImage(Url newImageUrl)
        {
            ImageUrl = newImageUrl;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}