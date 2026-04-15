using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Area
{
    public class AreaEditColumnDto
    {
        public required Guid Id { get; set; }
        public required Guid AreaCenterId { get; set; }
    }
}
