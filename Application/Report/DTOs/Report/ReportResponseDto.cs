using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.DTOs.Report
{
    public class ReportResponseDto
    {
       public required Guid Id { get; set; }
       public required Guid AreaId { get; set; }
       public required Guid CityId { get; set; }
       public required Guid StreetId { get; set; }
       public required Guid WareHouseId { get; set; }
       public required string phone { get; set; }
       public required string email { get; set; }
       public DateTime date { get; set; }
    }
}
