using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.City;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.City
{
    public class EditCity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required CityEditDto CityEditDto { get; set; }
        }   
        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([request.CityEditDto.Id], cancellationToken);
                if (city == null) return Result<Unit>.Failure("City not found", 404);
                mapper.Map(request.CityEditDto, city);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update city", 400);
                return Result<Unit>.Success(Unit.Value);
            }
        }   
    }
}
