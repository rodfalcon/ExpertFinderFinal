using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
            src.Photos.FirstOrDefault( x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>().ForMember(x => x.SendPhotoUrl, y => y.MapFrom(src => src.Sender.Photos.FirstOrDefault(z =>  z.IsMain).Url))
            .ForMember(x => x.RecipientPhotoUrl, y => y.MapFrom(src => src.Recipient.Photos.FirstOrDefault(z =>  z.IsMain).Url));
        }
    }
}