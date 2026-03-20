using AutoTrust.Domain.Enums;
using AutoTrust.Domain.ValueObjects;

namespace AutoTrust.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public BirthDate BirthDate { get; private set; }
        public Url? AvatarUrl { get; private set; } = null;
        public Gender Gender { get; private set; }
        public string AboutInfo { get; private set; } = string.Empty;
        public Account? Account { get; private set; }
        public int CityId { get; private set; }
        public City City { get; private set; }
        public ICollection<Listing> Listings { get; private set; } = [];
        public ICollection<ChatParticipant> ChatParticipants { get; private set; } = [];
        public ICollection<CarOwnership> CarOwnerships { get; private set; } = [];
        public ICollection<Comment> Comments { get; private set; } = [];
        public ICollection<Follow> Follows { get; private set; } = [];
        public ICollection<Notification> Notifications { get; private set; } = [];
        public ICollection<Reaction> Reactions { get; private set; } = [];
        public ICollection<Review> Reviews { get; private set; } = [];

        public bool IsDeleted { get; private set; } = false;
        public bool IsBlocked { get; private set; } = false;

        private User() { }

        public User
        (
            string name,
            BirthDate birthDate,
            Gender gender,
            string aboutInfo,
            int cityId
        )
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (string.IsNullOrWhiteSpace(aboutInfo))
                throw new ArgumentException("AboutInfo cannot be empty!");

            if (cityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(cityId), cityId, "CityId must be positive!");

            Name = name;
            BirthDate = birthDate;
            Gender = gender;
            AboutInfo = aboutInfo;
            CityId = cityId;
        }

        public void ChangeAvatar(Url? newAvatarUrl) => AvatarUrl = newAvatarUrl;

        public void UpdateInfo
        (
            string? name,
            BirthDate? birthDate,
            Gender? gender,
            string? aboutInfo,
            int? cityId
        )
        {
            if (name != null && string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty!");

            if (aboutInfo != null && string.IsNullOrWhiteSpace(aboutInfo))
                throw new ArgumentException("AboutInfo cannot be empty!");

            if (cityId <= 0)
                throw new ArgumentOutOfRangeException(nameof(cityId), cityId, "CityId must be positive!");

            Name = name ?? Name;
            BirthDate = birthDate ?? BirthDate;
            Gender = gender ?? Gender;
            AboutInfo = aboutInfo ?? AboutInfo;
            CityId = cityId ?? CityId;
        }

        public void Delete() => IsDeleted = true;
        public void Block() => IsBlocked = true;
        public void UnBlock() => IsBlocked = false;
    }
}
