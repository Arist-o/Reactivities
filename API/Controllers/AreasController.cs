using Application.Report.DTOs;
using Microsoft.AspNetCore.Mvc;
using Application.Report.Queries;
using Application.Report.Commands;
namespace API.Controllers
{
    public class AreasController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<AreaResponseDto>>> GetAreas()
        {
            return HandleResult(await Mediator.Send(new GetArea.Query()));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateArea([FromBody] AreaCreateDto areaCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateArea.Command { AreaCreateDto = areaCreateDto }));
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> EditArea(string Id, [FromBody] AreaEditDto areaEditDto)
        {
            areaEditDto.Id = Id;
            return HandleResult(await Mediator.Send(new EditArea.Command { AreaEditDto = areaEditDto }));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteArea(string Id)
        {
            return HandleResult(await Mediator.Send(new DeleteArea.Command { Id = Id }));
        }
    }
}
