using Application.Core;
using Application.Report.DTOs.Report;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Queries
{
    public class GetReport
    {
        public class Query : IRequest<Result<List<ReportResponseDto>>>
        {
            
        }
        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Query, Result<List<ReportResponseDto>>>
        {
            public async Task<Result<List<ReportResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var reports = await context.Reports
                    .ProjectTo<ReportResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<ReportResponseDto>>.Success(reports);
            }
        }
    }
}
