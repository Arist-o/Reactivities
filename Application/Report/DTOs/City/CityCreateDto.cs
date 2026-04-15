using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.City
{
    public class CityCreateDto
    {
        public required string Description { get; set; }

        public required Guid AreaId { get; set; }
    }


}
