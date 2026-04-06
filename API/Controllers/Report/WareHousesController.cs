using Application.Report.Commands.WareHouse;
using Application.Report.DTOs.WareHouse;
using Application.Report.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Report
{
    public class WareHousesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<WareHouseResponseDto>>> GetWareHouses()
        {
            return HandleResult(await Mediator.Send(new GetWareHouse.Query()));
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> createWareHouse([FromBody] WareHouseCreateDto wareHouseCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateWareHouse.Command { wareHouseCreateDto = wareHouseCreateDto }));
        }

        [HttpPut("{Id}")]
        public async Task<ActionResult> EditWareHouse(Guid Id, [FromBody] WareHouseEditDto wareHouseEditDto)
        {
            wareHouseEditDto.Id = Id;
            return HandleResult(await Mediator.Send(new EditWareHouse.Command { WareHouseEditDto = wareHouseEditDto }));
        }

        [HttpPatch("{Id}/set-city")]
        public async Task<ActionResult> EditColumnWareHouse(Guid Id, [FromBody] Guid cityId)
        {
            return HandleResult(await Mediator.Send(new EditColumnWareHouse.Command { WareHouseEditColumnDto = new WareHouseEditColumnDto { Id = Id, CityId = cityId } }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteWareHouse(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteWareHouse.Command { Id = Id }));
        }
    }
}
