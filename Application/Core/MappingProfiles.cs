using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Domain.Activity, Domain.Activity>();
        }
    }
}
