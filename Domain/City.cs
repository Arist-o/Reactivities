using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class City
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string AreaId { get; set; }
        public Area area { get; set; } = null!;

        public ICollection<Street> streets { get; set; } = [];

        public ICollection<WareHouse> wareHouses { get; set; } = [];
    }
}
