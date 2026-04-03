using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain
{
    public class Area
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string? AreaCenterId { get; set; } 
        public City? area_center { get; set; }

        public ICollection<City> Cities { get; set; } = [];


    }
}
