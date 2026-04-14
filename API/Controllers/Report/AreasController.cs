using Microsoft.AspNetCore.Mvc;
using Application.Report.Queries;
using Application.Report.DTOs.Area;
using Application.Report.Commands.Area;
namespace API.Controllers.Report
{
    public class AreasController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<AreaResponseDto>>> GetAreas()
        {
            return HandleResult(await Mediator.Send(new GetArea.Query()));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateArea([FromBody] AreaCreateDto areaCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateArea.Command { AreaCreateDto = areaCreateDto }));
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> EditArea(Guid Id, [FromBody] AreaEditDto areaEditDto)
        {
            areaEditDto.Id = Id;
            return HandleResult(await Mediator.Send(new EditArea.Command { AreaEditDto = areaEditDto }));
        }

        [HttpPatch("{Id}/set-center")]
        public async Task<ActionResult> EditColumnArea(Guid Id, [FromBody] Guid areaCenterId)
        {
            return HandleResult(await Mediator.Send(new EditColumnArea.Command { AreaEditColumnDto = new AreaEditColumnDto { Id = Id, AreaCenterId = areaCenterId } }));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteArea(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteArea.Command { Id = Id }));
        }
        [HttpPost("with-center")] 
        public async Task<ActionResult<Guid>> CreateAreaWithCenter([FromBody] CreateAreaWithCenter.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpPost("bulk")]
        public async Task<ActionResult> CreateAreasBulk([FromBody] CreateAreasBulk.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}
