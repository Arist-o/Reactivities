using Application.Core;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Application.Report.Commands.WareHouse
{
    public class EditColumnWareHouse
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required WareHouseEditColumnDto WareHouseEditColumnDto { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = await context.WareHouses.FindAsync([ request.WareHouseEditColumnDto.Id ], cancellationToken);

                if(wareHouse == null) return Result<Unit>.Failure("Not found", 404);

                var cityExists = await context.Cities.AnyAsync(a => a.Id == request.WareHouseEditColumnDto.CityId, cancellationToken);
                if (!cityExists) return Result<Unit>.Failure("City not found", 404);

                wareHouse.CityId = request.WareHouseEditColumnDto.CityId;



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
