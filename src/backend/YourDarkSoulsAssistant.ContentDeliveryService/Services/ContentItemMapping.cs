using AutoMapper;
using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class ContentItemProfile : Profile
{
    public ContentItemProfile()
    {
        CreateMap<ContentItem, ContentItemDTO>();
        
        CreateMap<InputContentItemDTO, ContentItem>()
            .ForMember(dest => dest.PublicRoute, opt => opt.MapFrom(src => src.Route));
    }
}
