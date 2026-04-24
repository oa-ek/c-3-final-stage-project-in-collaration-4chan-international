using AutoMapper;
using YourDarkSoulsAssistant.UserService.DTOs.Roles;
using YourDarkSoulsAssistant.UserService.DTOs.Users;
using YourDarkSoulsAssistant.UserService.Models;

namespace YourDarkSoulsAssistant.UserService.Services;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        // ==========================================
        // ROLES
        // ==========================================

        // Role -> RoleDTO
        CreateMap<Role, RoleDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

        // CreateRoleDTO -> Role
        CreateMap<CreateRoleDTO, Role>();

        // UpdateRoleDTO -> Role (Ігноруємо null значення при оновленні)
        CreateMap<UpdateRoleDTO, Role>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        // ==========================================
        // USERS
        // ==========================================

        // User -> UserDTO
        CreateMap<User, UserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))

            // Заглушка, якщо BuildCounts не зберігається в БД. 
            // Якщо є в БД, то просто видали цей рядок, AutoMapper змапить автоматично
            .ForMember(dest => dest.BuildCounts, opt => opt.MapFrom(src => 23235))

            // Безпечна обробка null для аватарки та ковенанту (якщо в БД вони null, віддамо дефолт)
            .ForMember(dest => dest.AvatarPath, opt => opt.MapFrom(src => src.AvatarPath ?? string.Empty))
            .ForMember(dest => dest.Covenant,
                opt => opt.MapFrom(src => src.Covenant ?? "Way of White")) // Дефолтний ковенант :)

            // Перевіряємо IsAdmin. EF Core завантажить Role завдяки ProjectTo, тому NullReferenceException тут не буде
            .ForMember(dest => dest.IsAdmin,
                opt => opt.MapFrom(src => src.Roles != null && src.Roles.Any(r => r.Name == "Admin")))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));
        
        CreateMap<User, SmallUserDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.IsAdmin,
                opt => opt.MapFrom(src => src.Roles != null && src.Roles.Any(r => r.Name == "Admin")))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name)));
        
        // CreateUserDTO -> User
        CreateMap<CreateUserDTO, User>();
        
        // UpdateUserDTO -> User
        CreateMap<UpdateUserDTO, User>()
            // Магія часткового оновлення: якщо в UpdateUserDTO поле null, воно НЕ перезапише те, що зараз є в User
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}