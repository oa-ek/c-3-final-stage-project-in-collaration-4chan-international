using AutoMapper;
using YourDarkSoulsAssistant.ContentDeliveryService.DTOs.Routes;
using YourDarkSoulsAssistant.ContentDeliveryService.Models;

namespace YourDarkSoulsAssistant.ContentDeliveryService.Services;

public class ContentItemProfile : Profile
{
    public ContentItemProfile()
    {
        CreateMap<ContentItem, ContentItemDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        
        CreateMap<InputContentItemDTO, ContentItem>()
            .ForMember(dest => dest.PublicRoute, opt => opt.MapFrom(src => src.Route));
    }
}
