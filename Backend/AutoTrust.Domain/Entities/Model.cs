namespace AutoTrust.Domain.Entities
{
    public class Model
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int BrandId { get; private set; }
        public Brand Brand { get; private set; }
        public bool IsActive { get; private set; }

        private Model() { }

        public Model
        (
            string name, 
            string description, 
            int brandId 
        )
        {
            Name = name;
            Description = description;
            BrandId = brandId;
            IsActive = true;
        }
    }
}