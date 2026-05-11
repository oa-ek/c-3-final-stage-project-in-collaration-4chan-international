using AutoMapper;
using YourDarkSoulsAssistant.UsersService.DTOs.Roles;
using YourDarkSoulsAssistant.UsersService.DTOs.Users;
using YourDarkSoulsAssistant.UsersService.Models;

namespace YourDarkSoulsAssistant.UsersService.Services;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<CreateRoleRequestDTO, Role>();
        
        CreateMap<UpdateRoleRequestDTO, Role>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        
        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))

            // Заглушка, якщо BuildCounts не зберігається в БД. 
            // Якщо є в БД, то просто видали цей рядок, AutoMapper змапить автоматично
            .ForMember(dest => dest.BuildCounts, opt => opt.MapFrom(src => 23235))

            // Безпечна обробка null для аватарки та ковенанту (якщо в БД вони null, віддамо дефолт)
            .ForMember(dest => dest.AvatarPath, opt => opt.MapFrom(src => src.AvatarPath))
            .ForMember(dest => dest.Covenant,
                opt => opt.MapFrom(src => src.Covenant ?? "Way of White"))
            
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
        
        CreateMap<User, SmallUserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));
        
        // CreateUserRequestDTO -> User
        CreateMap<CreateUserRequestDTO, User>();
        
        // UpdateUserRequestDTO -> User
        CreateMap<UpdateUserRequestDTO, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}