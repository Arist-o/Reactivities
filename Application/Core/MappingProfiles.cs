using Application.Activities.DTOs;
using AutoMapper;
using System.Linq;
using Domain;
using Application.Profiles.DTOs;
using Application.Report.DTOs;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string? currentUserId = null;
            CreateMap<Domain.Activity, Domain.Activity>();
            CreateMap<CreateActivityDto, Domain.Activity>();
            CreateMap<EditActivityDto, Domain.Activity>();
            CreateMap<City, CityDto>();
            CreateMap<Area, AreaResponseDto>();
            CreateMap<Street, StreetDto>();
            CreateMap<WareHouse, WareHouseDto>();
            CreateMap<Domain.Report, ReportDto>();

            CreateMap<AreaCreateDto, Domain.Area>();
            CreateMap<AreaEditDto, Domain.Area>();


            CreateMap<Domain.Activity, ActivityDto>()
                .ForMember(d => d.HostDisplayName, o => o.MapFrom(s =>
                    s.Attendees.FirstOrDefault(x => x.IsHost).User.DisplayName));
       
            CreateMap<ActivityAttendee, UserProfile>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.User.Followers.Count))
                .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.User.Followings.Count)) 
                .ForMember(d => d.Following, o => o.MapFrom(s =>
                    s.User.Followers.Any(x => x.Observer.Id == currentUserId)));

            CreateMap<User, UserProfile>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.Followings.Count)) // ВИПРАВЛЕНО
                .ForMember(d => d.Following, o => o.MapFrom(s =>
                    s.Followers.Any(x => x.Observer.Id == currentUserId)));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));
        }
    }
}