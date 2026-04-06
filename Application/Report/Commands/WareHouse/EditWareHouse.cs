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
    public class EditWareHouse
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required WareHouseEditDto WareHouseEditDto { get; set; }
        }
        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = context.WareHouses.FindAsync([request.WareHouseEditDto.Id], cancellationToken).Result;

                if (wareHouse == null) return Result<Unit>.Failure("WareHouse not found", 404);

                var cityExists = await context.Cities.AnyAsync(a => a.Id == request.WareHouseEditDto.CityId, cancellationToken);
                if (!cityExists) return Result<Unit>.Failure("City not found", 404);

                mapper.Map(request.WareHouseEditDto, wareHouse);

                var result = await context.SaveChangesAsync(cancellationToken) > 0; 

                if(!result) return Result<Unit>.Failure("Failed to update WareHouse", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
