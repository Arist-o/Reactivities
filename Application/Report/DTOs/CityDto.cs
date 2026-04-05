using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class CityDto
    {
        public required string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string AreaId { get; set; }
        public string AreaName { get; set; } 
    }
}
