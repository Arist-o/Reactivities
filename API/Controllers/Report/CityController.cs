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
        public async Task<ActionResult<Guid>> CreateCity([FromBody] CityCreateDto cityCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateCity.Command { CityCreateDto = cityCreateDto }));
        }


        [HttpPut("{Id}")]
        public async Task<ActionResult> EditCity(Guid Id, [FromBody] CityEditDto cityEditDto)
        {
            cityEditDto.Id = Id;
            return HandleResult(await Mediator.Send(new EditCity.Command { CityEditDto = cityEditDto }));
        }


        [HttpPatch("{Id}/set-area")]
        public async Task<ActionResult> EditColumnCity(Guid Id, [FromBody] Guid areaId)
        {
            return HandleResult(await Mediator.Send(new EditColumnCity.Command { CityEditColumnDto = new CityEditColumnDto { Id = Id, AreaId = areaId } }));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteCity(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteCity.Command { Id = Id }));
        }
    }
}
