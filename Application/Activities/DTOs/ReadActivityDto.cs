using Application.Profiles.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Activities.DTOs
{
    public class ReadActivityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Search { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 50, ErrorMessage = "Page size must be beetween 1 and 50")]
        public int PageSize { get; set; } = 10;

        public string[]? ids { get; set; }
    }
}
