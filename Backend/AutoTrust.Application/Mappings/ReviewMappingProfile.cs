using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.ReviewDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile() 
        {
            CreateMap<Review, AdminReviewByUserDto>();
            CreateMap<Review, AdminReviewByUserListItemDto>();
            CreateMap<Review, AdminReviewToUserDto>();
            CreateMap<Review, AdminReviewToUserListItemDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Review, ReviewListItemDto>();
            CreateMap<CreateReviewDto, Review>();
            CreateMap<Review, CreatedReviewDto>();
        }
    }
}

