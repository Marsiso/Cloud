using AutoMapper;
using Cloud.Core.Application.Users.Commands;
using Cloud.Domain.Application.Models;

namespace Cloud.Application.Application.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, User>()
            .ForMember(member => member.UserCreatedBy, options => options.Ignore())
            .ForMember(member => member.UserUpdatedBy, options => options.Ignore());

        CreateMap<User, CreateUserCommand>()
            .ForMember(member => member.Password, options => options.Ignore())
            .ReverseMap()
            .ForMember(member => member.Password, options => options.Ignore());
    }
}
