using Application.Core;
using Application.Report.DTOs.City;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace Application.Report.Commands.City
{
    public class EditColumnCity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required CityEditColumnDto CityEditColumnDto { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([request.CityEditColumnDto.Id], cancellationToken);

                
                if (city == null) return Result<Unit>.Failure("City not found", 404);

                var areaExists = await context.Areas.AnyAsync(a => a.Id == request.CityEditColumnDto.AreaId, cancellationToken);
                if (!areaExists) return Result<Unit>.Failure("Area not found", 404);

                city.AreaId = request.CityEditColumnDto.AreaId;
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
