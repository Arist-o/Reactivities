using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Application.Activities.Queries
{
    public class GetActivityMultipleSearchDetails
    {
        public class Query : IRequest<Result<List<ActivityDto>>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public string? Search { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.StartDate != default && request.EndDate != default && request.StartDate > request.EndDate)
                {
                    return Result<List<ActivityDto>>.Failure("Start date cannot be after end date", 400);
                }

                if (request.EndDate != default)
                {
                    request.EndDate = request.EndDate.Date.AddDays(1).AddTicks(-1);
                }

                var query = context.Activities.AsQueryable();

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

                var activities = await query.ToListAsync(cancellationToken);

                if (activities.Count == 0)
                {
                    return Result<List<ActivityDto>>.Failure("Activities not found", 404);
                }

                var result = mapper.Map<List<ActivityDto>>(activities);

                return Result<List<ActivityDto>>.Success(result);
            }
        }
    }
}
