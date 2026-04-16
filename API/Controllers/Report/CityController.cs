using Application.Report.DTOs.City;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Report.Queries;
using Application.Report.Commands.City;
using Microsoft.Identity.Client;

namespace API.Controllers.Report
{
    public class CityController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<CityResponseDto>>> GetCity()
        {
            return HandleResult(await Mediator.Send(new GetCity.Query()));
        }

        [HttpPost]
        public async Task<ActionResult<CityResponseDto>> CreateCity([FromBody] CityCreateDto cityCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateCity.Command { CityCreateDto = cityCreateDto }));
        }


        [HttpPut]
        public async Task<ActionResult<CityResponseDto>> EditCity( [FromBody] CityEditDto cityEditDto)
        {

            return HandleResult(await Mediator.Send(new EditCity.Command { CityEditDto = cityEditDto }));
        }


        [HttpPatch("set-area")]
        public async Task<ActionResult<CityResponseDto>> EditColumnCity([FromBody] CityEditColumnDto cityEditColumnDto)
        {
            return HandleResult(await Mediator.Send(new EditColumnCity.Command { CityEditColumnDto = cityEditColumnDto }));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteCity(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteCity.Command { Id = Id }));
        }
    }
}
