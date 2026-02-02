using AutoTrust.Domain.Enums;

namespace AutoTrust.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateOnly BirthDate { get; private set; }
        public string AvatarUrl { get; private set; }
        public Gender Gender { get; private set; }
        public string AboutInfo { get; private set; } = string.Empty;
        public Account? Account { get; private set; }
        public int? CountryId { get; private set; }
        public Country? Country { get; private set; }
        public int? CityId { get; private set; }
        public City? City { get; private set; }
        public ICollection<Listing> Listings { get; private set; } = [];
        public ICollection<ChatParticipant> ChatParticipants { get; private set; } = [];
        public bool IsDeleted { get; private set; } = false;

        private User() { }

        public User
        (
            string name,
            DateOnly birthDate,
            string avatarUrl,
            Gender gender,
            string aboutInfo,
            int countryId,
            int cityId
        )
        {
            Name = name; 
            BirthDate = birthDate;
            AvatarUrl = avatarUrl;
            Gender = gender;
            AboutInfo = aboutInfo;
            CountryId = countryId;
            CityId = cityId;
        }
    }
}
