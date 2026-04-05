using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Street
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string description { get; set; }

        public string streets_type { get; set; }

        public Guid CityId { get; set; }

        public City city { get; set; } = null!;

    }
}
