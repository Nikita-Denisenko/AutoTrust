namespace AutoTrust.Domain.Entities
{
    public class Country
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string PhoneCode { get; private set; }
        public string Currency { get; private set; }

        private Country() { }

        public Country(string name, string code, string phoneCode, string currency) 
        {
            Name = name;
            Code = code;
            PhoneCode = phoneCode;
            Currency = currency;
        }
    }
}