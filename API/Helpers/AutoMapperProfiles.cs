using System.Linq;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(prop => prop.ProfilePhoto, 
                    from => from.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsProfilePic).Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<Message, MessageDto>().ForMember(prop => prop.SenderProfilePicture, 
                from => from.MapFrom(src => src.Sender.Photos.FirstOrDefault(x => x.IsProfilePic).Url))
                .ForMember(prop => prop.ReceiverProfilePicture, 
                    from => from.MapFrom(src => src.Receiver.Photos.FirstOrDefault(x => x.IsProfilePic).Url));
        }
    }
}