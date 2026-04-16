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
        public async Task<ActionResult<WareHouseResponseDto>> createWareHouse([FromBody] WareHouseCreateDto wareHouseCreateDto)
        {
            return HandleResult(await Mediator.Send(new CreateWareHouse.Command { wareHouseCreateDto = wareHouseCreateDto }));
        }

        [HttpPut]
        public async Task<ActionResult<WareHouseResponseDto>> EditWareHouse([FromBody] WareHouseEditDto wareHouseEditDto)
        {
            return HandleResult(await Mediator.Send(new EditWareHouse.Command { WareHouseEditDto = wareHouseEditDto }));
        }

        [HttpPatch("set-city")]
        public async Task<ActionResult<WareHouseResponseDto>> EditColumnWareHouse([FromBody] WareHouseEditColumnDto wareHouseEditColumnDto)
        {
            return HandleResult(await Mediator.Send(new EditColumnWareHouse.Command { WareHouseEditColumnDto = wareHouseEditColumnDto }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteWareHouse(Guid Id)
        {
            return HandleResult(await Mediator.Send(new DeleteWareHouse.Command { Id = Id }));
        }
    }
}
