using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Area
{
    public class AreaEditDto
    {
        public required Guid Id { get; set; }

        public required string Description { get; set; }

        public required Guid AreaCenterId { get; set; } 
    }
}
