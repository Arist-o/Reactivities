using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Street
{
    public class StreetEditDto
    {
        public required Guid Id { get; set; }

        public required string description { get; set; }

        public string? streets_type { get; set; }

        public required Guid CityId { get; set; }
    }
}
