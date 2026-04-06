using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Street
{
    public class StreetResponseDto
    {
        public required Guid Id { get; set; }

        public required string Description { get; set; }

        public required string street_type { get; set; }

        public required Guid CityId { get; set; }
    }
}
