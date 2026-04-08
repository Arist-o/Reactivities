using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Activities.Commands
{
    public class UpdateAttendance
    {
        public class Commmand : IRequest<Result<Unit>>
        {
            public required string Id { get; set; }
        }
        public class Handler(IUserAccessor userAccessor, IAppDbContext context) 
            : IRequestHandler<Commmand, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Commmand request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                    .Include(x => x.Attendees)
                    .ThenInclude(x => x.User)
                    .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if(activity == null) return Result<Unit>.Failure("Activity not found", 404);

                var user = await userAccessor.GetUserAsync();

                var attendance = activity.Attendees.FirstOrDefault(x => x.User.UserName == user.UserName);
                var isHost = activity.Attendees.Any(x => x.IsHost && x.UserId == user.Id);

                if (attendance != null)
                {
                    if (isHost)
                    {
                        activity.IsCancelled = !activity.IsCancelled;
                    }
                    else
                    {
                        activity.Attendees.Remove(attendance);
                    }
                }
                else 
                {
                    activity.Attendees.Add(new Domain.ActivityAttendee
                    {
                        User = user,
                        Activity = activity,
                        IsHost = false
                    });
                }

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                return result
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Failed to update attendance", 500);
            }
        }
    }
}
