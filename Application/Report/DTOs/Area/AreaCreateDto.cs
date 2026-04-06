using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Area
{
    public class AreaCreateDto
    {
        public required string Description { get; set; }
        public Guid? AreaCenterId { get; set; }
    }
}
