using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class ReportDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string AreaId { get; set; } = string.Empty;

      
        public string CityId { get; set; } = string.Empty;


        public string StreetId { get; set; } = string.Empty;


        public string WareHouseId { get; set; } = string.Empty;



        public required string Phone { get; set; }

        public required string Email { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
