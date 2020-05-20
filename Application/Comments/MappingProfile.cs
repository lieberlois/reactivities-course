using System.Linq;
using AutoMapper;
using Domain;

namespace Application.Comments
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(x => x.Username, options => options.MapFrom(s => s.Author.UserName))
                .ForMember(x => x.DisplayName, options => options.MapFrom(s => s.Author.DisplayName))
                .ForMember(x => x.Image, options => options.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}