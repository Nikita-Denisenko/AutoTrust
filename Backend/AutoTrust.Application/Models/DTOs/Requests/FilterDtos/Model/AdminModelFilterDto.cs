namespace AutoTrust.Application.Models.DTOs.Requests.FilterDtos.Model
{
    public record AdminModelFilterDto : ModelFilterDto
    {
        public bool? IsActive { get; init; }

        public AdminModelFilterDto(
            int Page = 1,
            int Size = 20,
            int? BrandId = null,
            string? SearchText = null,
            bool SortByAsc = true,
            bool? IsActive = null
        ) : base(Page, Size, BrandId, SearchText, SortByAsc)
        {
            this.IsActive = IsActive;
        }
    }
}