using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Application.Report.DTOs.WareHouse
{
    public class WareHouseCreateDto
    {
        public required string description { get; set; }

        public required int number { get; set; }

        public Guid? CityId { get; set; }
    }
}
