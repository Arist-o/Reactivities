using Application.Core;
using Application.Report.DTOs.Report;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Application.Report.Commands.Report
{
    public class EditReport
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required ReportEditDto ReportEditDto { get; set; }
        }

        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var report = await context.Reports.FindAsync(request.ReportEditDto.Id);

                if(report == null) return Result<Unit>.Failure("Report not found", 404);

                var areaExists = await context.Cities.AnyAsync(a => a.Id == request.ReportEditDto.AreaId, cancellationToken);
                if (!areaExists) return Result<Unit>.Failure("Area not found", 404);

                var cityExists = await context.Cities.AnyAsync(a => a.Id == request.ReportEditDto.CityId, cancellationToken);
                if (!cityExists) return Result<Unit>.Failure("City not found", 404);

                var streetExists = await context.Cities.AnyAsync(a => a.Id == request.ReportEditDto.StreetId, cancellationToken);
                if (!streetExists) return Result<Unit>.Failure("Street not found", 404);

                var wareHouseExists = await context.Cities.AnyAsync(a => a.Id == request.ReportEditDto.WareHouseId, cancellationToken);
                if (!wareHouseExists) return Result<Unit>.Failure("WareHouse not found", 404);

                mapper.Map(request.ReportEditDto, report);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if(!result) return Result<Unit>.Failure("Failed to update report", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
