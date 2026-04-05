using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Domain
{
    public class WareHouse
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public required string description { get; set; }

        public Guid CityId { get; set; }

        public City city { get; set; } = null!;

        public required int number { get; set; }
    }
}
