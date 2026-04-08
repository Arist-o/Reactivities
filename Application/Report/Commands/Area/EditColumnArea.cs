using Application.Core;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.Report.DTOs.Area;
using Application.Interfaces;

namespace Application.Report.Commands.Area
{
    public class EditColumnArea
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required AreaEditColumnDto AreaEditColumnDto { get; set; }
        }

        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = await context.Areas.FindAsync([request.AreaEditColumnDto.Id], cancellationToken);

                if (area == null) return Result<Unit>.Failure("Area not found", 404);

                var cityExists = await context.Cities.AnyAsync(c => c.Id == request.AreaEditColumnDto.AreaCenterId, cancellationToken);
                if (!cityExists) return Result<Unit>.Failure("City not found", 404);

                area.AreaCenterId = request.AreaEditColumnDto.AreaCenterId;

                try
                {
                    await context.SaveChangesAsync(cancellationToken);
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure("Database error: " + ex.Message, 400);
                }
            }
        }
    }
}
