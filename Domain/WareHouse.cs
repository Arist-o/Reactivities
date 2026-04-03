using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace Domain
{
    public class WareHouse
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string description { get; set; }

        public string CityId { get; set; }

        public City city { get; set; } = null!;

        public required int number { get; set; }
    }
}
