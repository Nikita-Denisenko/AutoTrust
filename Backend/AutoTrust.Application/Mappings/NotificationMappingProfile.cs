using AutoMapper;
using AutoTrust.Application.Models.DTOs.Requests.CreateDtos;
using AutoTrust.Application.Models.DTOs.Responses.CreatedDtos;
using AutoTrust.Application.Models.DTOs.Responses.ReadDtos.NotificationDtos;
using AutoTrust.Domain.Entities;

namespace AutoTrust.Application.Mappings
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile() 
        {
            CreateMap<Notification, AdminNotificationDto>();
            CreateMap<Notification, AdminNotificationListItemDto>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<Notification, NotificationListItemDto>();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<Notification, CreatedNotificationDto>();
        }
    }
}
