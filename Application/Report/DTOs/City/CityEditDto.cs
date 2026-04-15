using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.City
{
    public class CityEditDto
    {
        public required Guid Id { get; set; }

        public required string Description { get; set; }

        public required Guid AreaId { get; set; }
    }
}
