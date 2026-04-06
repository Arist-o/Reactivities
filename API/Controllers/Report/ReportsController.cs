using Microsoft.AspNetCore.Mvc;
using Application.Report.Queries;
using Application.Report.DTOs.Report;
using Application.Report.Commands.Report;
namespace API.Controllers.Report
{
    public class ReportsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ReportResponseDto>> GetReport()
        {
            return HandleResult(await Mediator.Send(new GetReport.Query()));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> createReport([FromBody] ReportCreateDto reportCreate)
        {
            return HandleResult(await Mediator.Send(new CreateReport.Command { reportCreateDto = reportCreate }));
        }

        [HttpPut]
        public async Task<ActionResult> EditReport([FromBody] ReportEditDto reportEdit)
        {
            return HandleResult(await Mediator.Send(new EditReport.Command { ReportEditDto = reportEdit }));
        }

        [HttpDelete]

        public async Task<ActionResult> DeleteReport(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteReport.Command { Id = Id }));
        }
    }
}
