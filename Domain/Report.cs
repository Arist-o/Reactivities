using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AreaId { get; set; }

        public Area? Area { get; set; }

        public Guid CityId { get; set; }

        public City City { get; set; } = null!;

        public Guid StreetId { get; set; }

        public Street Street { get; set; } = null!;

        public Guid WareHouseId { get; set; } 

        public WareHouse WareHouse { get; set; } = null!;


        public required string phone { get; set; }

        public required string email { get; set; }

        public DateTime date { get; set; } = DateTime.UtcNow;
    }
}
