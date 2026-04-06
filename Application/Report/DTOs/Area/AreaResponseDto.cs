using Application.Report.DTOs.City;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Area
{
    public class AreaResponseDto
    {
        public  Guid Id { get; set; }  

        public  required string Description { get; set; }

        public required CityResponseDto City { get; set; }


    }
}
