using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class StreetDto
    {
        public Guid Id { get; set; } 

        public required string description { get; set; }

        public string streets_type { get; set; }

        public Guid CityId { get; set; }

    }
}
