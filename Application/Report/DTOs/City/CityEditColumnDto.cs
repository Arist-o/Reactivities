using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.City
{
    public class CityEditColumnDto
    {
        public required Guid Id { get; set; }

        public required Guid AreaId { get; set; }
    }
}
