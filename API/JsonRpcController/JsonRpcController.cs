using API.Controllers;
using Application.Activities.DTOs;
using Application.Core;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace API.JsonRpcController
{
    public class RpcController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Dispatch([FromBody]RpcRequestDto request)
        {
            string safeMethodName = request.Method?.Trim() ?? string.Empty;
            var commandType = CommandRegistry.GetCommandType(safeMethodName);

            if (commandType == null)
            {
                return BadRequest(new { Success = false, Error = $"Метод '{request.Method}' не знайдено." });
            }
            try
            {
                var command = request.Params.Deserialize(commandType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var result = await Mediator.Send(command);

                return Ok(new { Success = true, Data = result });
            }
            catch (JsonException)
            {
                return BadRequest(new { Success = false, Error = "Невалідні параметри для цього методу." });
            }
        }
    }
}
