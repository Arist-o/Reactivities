using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Street
{
    public class StreetCreateDto
    {
        public required string description { get; set; }

        public string? streets_type { get; set; }

        public required Guid CityId { get; set; }
    }
}
