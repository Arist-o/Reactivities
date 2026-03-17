using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace Application.Activities.Queries
{
    public class GetActivityMultipleSearchDetails
    {
        public class Query : IRequest<Result<PagedResult<ActivityDto>>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public string? Search { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
            public int PageNumber { get; set; } = 1;

            [Range(1, 50, ErrorMessage = "Page size must be beetween 1 and 50")]
            public int PageSize { get; set; } = 10;
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<PagedResult<ActivityDto>>>
        {
            public async Task<Result<PagedResult<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.StartDate != default && request.EndDate != default && request.StartDate > request.EndDate)
                {
                    return Result<PagedResult<ActivityDto>>.Failure("Start date cannot be after end date", 400);
                }

                if (request.EndDate != default)
                {
                    request.EndDate = request.EndDate.Date.AddDays(1).AddTicks(-1);
                }

                var query = context.Activities.AsNoTracking();

                if (request.StartDate != default)
                {
                    query = query.Where(x => x.Date >= request.StartDate);
                }

                if (request.EndDate != default)
                {
                    query = query.Where(x => x.Date <= request.EndDate);
                }

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    query = query.Where(x => x.Title.Contains(request.Search)
                                          || x.Description.Contains(request.Search)
                                          || x.City.Contains(request.Search)
                                          || x.Venue.Contains(request.Search));
                }
                var totalCount = await query.CountAsync(cancellationToken);
               
                if (totalCount == 0)
                {
                    return Result<PagedResult<ActivityDto>>.Failure("Activities not found", 404);
                }

                var activities = await query.ToListAsync(cancellationToken);

                var items = await query
                 .Include(x => x.Attendees)
                 .OrderBy(x => x.Date)
                 .ThenBy(x => x.Title)
                 .Skip((request.PageNumber - 1) * request.PageSize)
                 .Take(request.PageSize)
                 .ToListAsync(cancellationToken);

                var dtos = mapper.Map<List<ActivityDto>>(items);
                var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

                var metadata = new PaginationMetadata
                {
                    TotalCount = totalCount,
                    PageSize = request.PageSize,
                    CurrentPage = request.PageNumber,
                    TotalPages = totalPages,
                    HasNext = request.PageNumber < totalPages,
                    HasPrevious = request.PageNumber > 1
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
