using Microsoft.AspNetCore.Mvc;
using Application.Core;
using Domain;
using MediatR;
using Application.Activities.Queries;
using Application.Activities.DTOs;
using Application.Activities.Commands;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedList<ActivityDto,DateTime?>>> GetActivities(DateTime? cursor)
        {
            return HandleResult(await Mediator.Send(new GetActivityList.Query { Cursor = cursor }));
        }
        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult<List<ActivityDto>>> GetActivities([FromQuery] GetActivityMultipleSearchDetails.Query query)
        // {
        //     return HandleResult(await Mediator.Send(query));
        //}

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityDto>> GetActivityDetail(string id)
        {
            return HandleResult(await Mediator.Send(new GetActivityDetails.Query { Id = id }));
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateActivity(CreateActivityDto activityDto)
        {
            return HandleResult(await Mediator.Send(new Application.Activities.Commands.CreateActivity.Command { ActivityDto = activityDto }));
        }

        [HttpPut("{id}")]
        [Authorize(Policy ="IsActivityHost")]
        public async Task<ActionResult> EditActivity(string id,EditActivityDto activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new EditActivity.Command { ActivityDto = activity }));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "IsActivityHost")]
        public async Task<ActionResult> DeleteActivity(string id)
        {
            return HandleResult(await Mediator.Send(new Application.Activities.Commands.DeleteActivity.Command { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<ActionResult> Attend(string id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Commmand { Id = id }));
        }
    }
}
