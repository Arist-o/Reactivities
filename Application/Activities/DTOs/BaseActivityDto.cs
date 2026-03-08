using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Activities.DTOs
{
    public class BaseActivityDto
    {
        public required string Title { get; set; } = "";
        public DateTime Date { get; set; }

        public required string Description { get; set; } = string.Empty;

        public required string Category { get; set; } = "";


        // location props

        public required string City { get; set; } = "";

        public required string Venue { get; set; } = "";

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
