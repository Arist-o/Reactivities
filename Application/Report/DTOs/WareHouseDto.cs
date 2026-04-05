using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs
{
    public class WareHouseDto
    {
        public Guid Id { get; set; }

        public required string description { get; set; }

        public Guid CityId { get; set; }

        public required int number { get; set; }
    }
}
