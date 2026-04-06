using Application.Core;
using Application.Report.DTOs.Street;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Application.Report.Commands.Street
{
    public class EditColumnStreet
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required StreetEditColumnDto StreetEditColumnDto { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = context.Streets.Find(request.StreetEditColumnDto.Id);
                
                if(street == null ) return Result<Unit>.Failure("Not found streeet", 404);

                var cityExists = await context.Cities.AnyAsync(a => a.Id == request.StreetEditColumnDto.CityId, cancellationToken);
                if (!cityExists) return Result<Unit>.Failure("City not found", 404);


                street.CityId = request.StreetEditColumnDto.CityId;

               
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
