using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class Country
    {
        public int Id { get; private set; }
        public string EnName { get; private set; }
        public string RuName { get; private set; }
        public string Code { get; private set; }
        public Url FlagImageUrl { get; private set; }

        private Country() { }

        public Country
        (
            string enName, 
            string ruName,
            string code,
            Url flagImageUrl
        ) 
        {
            EnName = enName;
            RuName = ruName;
            Code = code;
            FlagImageUrl = flagImageUrl;
        }
    }
}