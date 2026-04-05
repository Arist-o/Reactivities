using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class WareHouseDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string CityId { get; set; }

        public required int number { get; set; }
    }
}
