using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Source, Target>
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                // .ForMember(target, options)
                .ForMember(d => d.Username, options => options.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, options => options.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Image, options => options.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.Following, o => o.MapFrom<FollowingResolver>());
        }
    }
}