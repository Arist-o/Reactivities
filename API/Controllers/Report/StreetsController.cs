using Application.Report.DTOs.Street;
using Microsoft.AspNetCore.Mvc;
using Application.Report.Queries;
using Application.Report.Commands.Street;
namespace API.Controllers.Report
{
    public class StreetsController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<StreetResponseDto>>> GetStreets()
        {
           return HandleResult(await Mediator.Send(new GetStreet.Query()));
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateStreet([FromBody] StreetCreateDto streetCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateStreet.Command { streetCreateDto = streetCreateDto }));
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult> EditStreet(Guid Id, [FromBody] StreetEditDto streetEditDto)
        {
            streetEditDto.Id = Id;
            return HandleResult(await Mediator.Send(new EditStreet.Command { StreetEditDto = streetEditDto }));
        }

        [HttpPatch("{Id}/set-city")]
        public async Task<ActionResult> EditColumnStreet(Guid Id, [FromBody] Guid cityId)
        {
            return HandleResult(await Mediator.Send(new EditColumnStreet.Command { StreetEditColumnDto = new StreetEditColumnDto { Id = Id, CityId = cityId } }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteStreet(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteStreet.Command { Id = Id }));
        }
    }
}
