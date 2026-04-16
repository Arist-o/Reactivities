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
        public async Task<ActionResult<StreetResponseDto>> CreateStreet([FromBody] StreetCreateDto streetCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateStreet.Command { streetCreateDto = streetCreateDto }));
        }
        [HttpPut]
        public async Task<ActionResult<StreetResponseDto>> EditStreet([FromBody] StreetEditDto streetEditDto)
        {
            return HandleResult(await Mediator.Send(new EditStreet.Command { StreetEditDto = streetEditDto }));
        }

        [HttpPatch("set-city")]
        public async Task<ActionResult<StreetResponseDto>> EditColumnStreet([FromBody] StreetEditColumnDto streetEditColumnDto)
        {
            return HandleResult(await Mediator.Send(new EditColumnStreet.Command { StreetEditColumnDto = streetEditColumnDto }));
        }

        [HttpDelete]
        public async Task<ActionResult<StreetResponseDto>> DeleteStreet(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteStreet.Command { Id = Id }));
        }
    }
}
