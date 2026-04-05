using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class AreaEditDto
    {
        public required Guid Id { get; set; }

        public required string Description { get; set; }

        public Guid? AreaCenteId { get; set; } 
    }
}
