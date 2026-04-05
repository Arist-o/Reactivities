using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class AreaEditDto
    {
        public required string Id { get; set; }

        public required string Description { get; set; }

        public string? AreaCenteId { get; set; } 
    }
}
