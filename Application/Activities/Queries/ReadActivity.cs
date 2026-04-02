using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Application.Activities.Queries
{
    public class ReadActivity
    {
        public class Command : IRequest<Result<Core.PagedResult<ActivityDto>>>
        {
            public required ReadActivityDto ReadActivityDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper, IUserAccessor userAccessor)
          : IRequestHandler<Command, Result<Core.PagedResult<ActivityDto>>>
        {                   
            public async Task<Result<Core.PagedResult<ActivityDto>>> Handle(Command request, CancellationToken cancellationToken)
            {



                if (request.ReadActivityDto.StartDate != default && request.ReadActivityDto.EndDate != default && request.ReadActivityDto.StartDate > request.ReadActivityDto.EndDate)
                {
                    return Result<Core.PagedResult<ActivityDto>>.Failure("Start date cannot be after end date", 400);

                }

                if (request.ReadActivityDto.EndDate != default)
                {
                    request.ReadActivityDto.EndDate = request.ReadActivityDto.EndDate.Date.AddDays(1).AddTicks(-1);
                }

                var query = context.Activities.AsNoTracking();

                if (request.ReadActivityDto.StartDate != default)
                {
                    query = query.Where(x => x.Date >= request.ReadActivityDto.StartDate);
                }

                if (request.ReadActivityDto.EndDate != default)
                {
                    query = query.Where(x => x.Date <= request.ReadActivityDto.EndDate);
                }

                if (!string.IsNullOrWhiteSpace(request.ReadActivityDto.Search))
                {
                    query = query.Where(x => x.Title.Contains(request.ReadActivityDto.Search)
                                          || x.Description.Contains(request.ReadActivityDto.Search)
                                          || x.City.Contains(request.ReadActivityDto.Search)
                                          || x.Venue.Contains(request.ReadActivityDto.Search));
                }
                if (request.ReadActivityDto.ids != null && request.ReadActivityDto.ids.Any())
                {
                    query = query.Where(activity =>
                        activity.Attendees.Any(attendee => request.ReadActivityDto.ids.Contains(attendee.UserId)));
                }
                var totalCount = await query.CountAsync(cancellationToken);

                if (totalCount == 0)
                {
                    return Result<Core.PagedResult<ActivityDto>>.Failure("Activities not found", 404);
                }


                if (!string.IsNullOrWhiteSpace(request.ReadActivityDto.ColumnName))
                {
                    string sortOrder = request.ReadActivityDto.AscDesc ? "ascending" : "descending";

                    query = query.OrderBy($"{request.ReadActivityDto.ColumnName} {sortOrder}");
                }
                else
                {
                    query = query.OrderBy(x => x.Title);
                }

                var currentUserId = userAccessor.GetUserId();

                var dtos = await query
                    .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUserId })
                    .Skip((request.ReadActivityDto.PageNumber - 1) * request.ReadActivityDto.PageSize)
                    .Take(request.ReadActivityDto.PageSize)
                    .ToListAsync(cancellationToken);
                var totalPages = (int)Math.Ceiling(totalCount / (double)request.ReadActivityDto.PageSize);

                var metadata = new PaginationMetadata
                {
                    TotalCount = totalCount,
                    PageSize = request.ReadActivityDto.PageSize,
                    CurrentPage = request.ReadActivityDto.PageNumber,
                    TotalPages = totalPages,
                    HasNext = request.ReadActivityDto.PageNumber < totalPages,
                    HasPrevious = request.ReadActivityDto.PageNumber > 1
                };

                var pagedResult = new Core.PagedResult<ActivityDto>
                {
                    Data = dtos,
                    Metadata = metadata
                };

                return Result<Core.PagedResult<ActivityDto>>.Success(pagedResult);
            }
        }
    }
}
