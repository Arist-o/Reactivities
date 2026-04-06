using Application.Report.DTOs.Area;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.City
{
    public class CityResponseDto
    {
        public Guid Id { get; set; }

        public required string Description { get; set; }

        public required AreaResponseDto Area { get; set; }
    }
}
