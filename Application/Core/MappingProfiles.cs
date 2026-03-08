using Application.Activities.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Domain.Activity, Domain.Activity>();
            CreateMap<CreateActivityDto, Domain.Activity>();
            CreateMap<EditActivityDto, Domain.Activity>();
        }
    }
}
