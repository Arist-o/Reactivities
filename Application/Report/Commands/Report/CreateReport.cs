using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Report;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Report
{
    public class CreateReport
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required ReportCreateDto reportCreateDto { get; set; }
        }
        public class Handler(IAppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
               var report = mapper.Map<Domain.Report>(request.reportCreateDto);
                context.Reports.Add(report);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Guid>.Failure("Failed to create report", 500);
                return Result<Guid>.Success(report.Id);
            }
        }
    }
}
