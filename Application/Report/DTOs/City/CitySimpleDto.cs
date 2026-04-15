using Application.Report.DTOs.Area;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.City
{
    public class CitySimpleDto
    {
        public required Guid Id { get; set; }    

        public required string Description { get; set; }   

        public required AreaSimpleDto Area { get; set; }
    }
}
