using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class AreaResponseDto
    {
        public  Guid Id { get; set; }  

        public  string Description { get; set; }

        public CityDto? City { get; set; }


    }
}
