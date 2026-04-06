using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Street
{
    public class StreetEditColumnDto
    {
        public required Guid Id { get; set; }

        public required Guid CityId { get; set; }   
    }
}
