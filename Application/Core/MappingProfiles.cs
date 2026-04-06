using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using Application.Report.DTOs;
using Application.Report.DTOs.Area;
using Application.Report.DTOs.City;
using Application.Report.DTOs.Report;
using Application.Report.DTOs.Street;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using Domain;
using System.Linq;


namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string? currentUserId = null;




            CreateMap<CreateActivityDto, Activity>();
            CreateMap<EditActivityDto, Activity>();
            
            
            CreateMap<Street, StreetDto>();
            CreateMap<WareHouse, WareHouseDto>();
            CreateMap<Domain.Report, ReportDto>();


            // area
            CreateMap<AreaCreateDto, Area>();
            CreateMap<AreaEditDto, Area>();
            CreateMap<Area, AreaResponseDto>();
            CreateMap<Area, AreaSimpleDto>();

            // city
            CreateMap<CityCreateDto, City>();
            CreateMap<CityEditDto, City>();
            CreateMap<City, CityResponseDto>();
            CreateMap<City, CitySimpleDto>();

            // Report
            CreateMap<ReportCreateDto, Domain.Report>();
            CreateMap<ReportEditDto, Domain.Report>();
            CreateMap<Domain.Report, ReportResponseDto>();

            // Street
            CreateMap<StreetCreateDto, Street>();
            CreateMap<StreetEditDto, Street>();
            CreateMap<StreetEditColumnDto, Street>(); 
            CreateMap<Street, StreetResponseDto>();

            // WareHouse
            CreateMap<WareHouseCreateDto, WareHouse>();
            CreateMap<WareHouseEditDto, WareHouse>();
            CreateMap<WareHouseEditColumnDto, WareHouse>(); 
            CreateMap<WareHouse, WareHouseResponseDto>();

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
                .ForMember(d => d.FollowingsCount, o => o.MapFrom(s => s.Followings.Count)) 
                .ForMember(d => d.Following, o => o.MapFrom(s =>
                    s.Followers.Any(x => x.Observer.Id == currentUserId)));

            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));
        }
    }
}