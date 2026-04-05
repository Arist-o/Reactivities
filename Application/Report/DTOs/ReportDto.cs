using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class ReportDto
    {
        public Guid Id { get; set; } 

        public Guid AreaId { get; set; } 

      
        public Guid CityId { get; set; } 


        public Guid StreetId { get; set; } 


        public Guid WareHouseId { get; set; } 



        public required string Phone { get; set; }

        public required string Email { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
