using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class City
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string description { get; set; }

        public Guid AreaId { get; set; }
        public Area area { get; set; } = null!;

        public ICollection<Street> streets { get; set; } = [];

        public ICollection<WareHouse> wareHouses { get; set; } = [];
    }
}
