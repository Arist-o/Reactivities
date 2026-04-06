using Application.Core;
using Application.Report.DTOs.City;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.City
{
    public class CreateCity
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required CityCreateDto CityCreateDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.CityCreateDto.AreaId == null || request.CityCreateDto.AreaId == Guid.Empty)
                {
                    return Result<Guid>.Failure("AreaId is required for standalone City creation", 400);
                }
                var city = mapper.Map<Domain.City>(request.CityCreateDto);
                context.Cities.Add(city);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Guid>.Failure("Failed to create city", 500);
                return Result<Guid>.Success(city.Id);
            }
        }
    }
}
