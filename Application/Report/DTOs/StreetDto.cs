using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class StreetDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string streets_type { get; set; }

        public string CityId { get; set; }

    }
}
