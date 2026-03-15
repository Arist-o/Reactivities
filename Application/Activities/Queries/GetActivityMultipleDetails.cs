using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Activities.Queries
{
    public class GetActivityMultipleDetails
    {
        public class Query : IRequest<Result<List<ActivityDto>>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate{ get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activity = new List<Domain.Activity>();
                if (request.StartDate == default || request.EndDate == default)
                {
                    activity = await context.Activities
                           .ToListAsync(cancellationToken);
                }
                else if (request.StartDate != default || request.EndDate == default)
                {
                    activity = await context.Activities
                           .Where(x => x.Date >= request.StartDate)
                           .ToListAsync(cancellationToken);
                }
                else if (request.StartDate == default || request.EndDate != default)
                {
                    activity = await context.Activities
                            .Where(x => x.Date <= request.EndDate)
                            .ToListAsync(cancellationToken);
                }
                else
                {
                    activity = await context.Activities
                            .Where(x => x.Date >= request.StartDate && x.Date <= request.EndDate)
                            .ToListAsync(cancellationToken);
                }
                if (activity == null) return Result<List<ActivityDto>>.Failure("Activity not found", 404);

                



                var result = mapper.Map<List<ActivityDto>>(activity);

                return Result<List<ActivityDto>>.Success(result);
            }
        }
    }

}
