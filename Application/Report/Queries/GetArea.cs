using Application.Core;
using Application.Report.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Queries
{
    public class GetArea
    {
        public class Query : IRequest<Result<List<AreaResponseDto>>>
        {

        }
        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Query, Result<List<AreaResponseDto>>>
        {
            public async Task<Result<List<AreaResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var areas = await context.Areas
                    .ProjectTo<AreaResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return Result<List<AreaResponseDto>>.Success(areas);
            }
        }
    }
}
