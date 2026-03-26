using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Activities.Commands
{
    public class ReadActivity
    {
        public class Command : IRequest<Result<PagedResult<ActivityDto>>>
        {
            public required ReadActivityDto ReadActivityDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
          : IRequestHandler<Command, Result<PagedResult<ActivityDto>>>
        {                   
            public async Task<Result<PagedResult<ActivityDto>>> Handle(Command request, CancellationToken cancellationToken)
            {

                var activityMap = mapper.Map<ReadActivityDto>(request.ReadActivityDto);


                if (activityMap.StartDate != default && activityMap.EndDate != default && activityMap.StartDate > activityMap.EndDate)
                {
                    return Result<PagedResult<ActivityDto>>.Failure("Start date cannot be after end date", 400);

                }

                if (activityMap.EndDate != default)
                {
                    activityMap.EndDate = activityMap.EndDate.Date.AddDays(1).AddTicks(-1);
                }

                var query = context.Activities.AsNoTracking();

                if (activityMap.StartDate != default)
                {
                    query = query.Where(x => x.Date >= activityMap.StartDate);
                }

                if (activityMap.EndDate != default)
                {
                    query = query.Where(x => x.Date <= activityMap.EndDate);
                }

                if (!string.IsNullOrWhiteSpace(activityMap.Search))
                {
                    query = query.Where(x => x.Title.Contains(activityMap.Search)
                                          || x.Description.Contains(activityMap.Search)
                                          || x.City.Contains(activityMap.Search)
                                          || x.Venue.Contains(activityMap.Search));
                }
                if (activityMap.ids != null && activityMap.ids.Any())
                {
                    query = query.Where(activity =>
                        activity.Attendees.Any(attendee => activityMap.ids.Contains(attendee.UserId)));
                }
                var totalCount = await query.CountAsync(cancellationToken);

                if (totalCount == 0)
                {
                    return Result<PagedResult<ActivityDto>>.Failure("Activities not found", 404);
                }

                var items = await query
                    .Include(x => x.Attendees)
                        .ThenInclude(xx => xx.User)
                    .OrderBy(x => x.Date)
                    .ThenBy(x => x.Title)
                    .Skip((activityMap.PageNumber - 1) * activityMap.PageSize)
                    .Take(activityMap.PageSize)
                    .ToListAsync(cancellationToken);





                var dtos = mapper.Map<List<ActivityDto>>(items);
                var totalPages = (int)Math.Ceiling(totalCount / (double)activityMap.PageSize);

                var metadata = new PaginationMetadata
                {
                    TotalCount = totalCount,
                    PageSize = activityMap.PageSize,
                    CurrentPage = activityMap.PageNumber,
                    TotalPages = totalPages,
                    HasNext = activityMap.PageNumber < totalPages,
                    HasPrevious = activityMap.PageNumber > 1
                };

                var pagedResult = new PagedResult<ActivityDto>
                {
                    Data = dtos,
                    Metadata = metadata
                };

                return Result<PagedResult<ActivityDto>>.Success(pagedResult);
            }
        }
    }
}
