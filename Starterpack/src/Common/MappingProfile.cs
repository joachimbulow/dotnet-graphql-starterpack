// For AutoMapper
using AutoMapper;
using Starterpack.User.Api.Inputs;
using Starterpack.User.Domain.Models;
using Starterpack.User.Persistance.Entities;
using Spazw.User.Api.Inputs;

namespace Starterpack.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserModel>();

            CreateMap<CreateUserInput, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}