using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Street
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string streets_type { get; set; }

        public string CityId { get; set; }

        public City city { get; set; } = null!;

    }
}
