using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class CityDto
    {
        public required Guid Id { get; set; }

        public required string description { get; set; }

        public Guid AreaId { get; set; }
        public string AreaName { get; set; } 
    }
}
