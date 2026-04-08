using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Area;
using Application.Report.DTOs.City;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.Area
{
    public class CreateAreaWithCenter
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required AreaCreateDto Area { get; set; }
            public required CityCreateDto City { get; set; }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
              

                var area = mapper.Map<Domain.Area>(request.Area);
                context.Areas.Add(area);

                await context.SaveChangesAsync(cancellationToken);

                var city = mapper.Map<Domain.City>(request.City);

                city.AreaId = area.Id;

                context.Cities.Add(city);
                await context.SaveChangesAsync(cancellationToken);

                area.AreaCenterId = city.Id;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Guid>.Failure("Failed to finalize area-city relationship", 400);

                return Result<Guid>.Success(area.Id);
            }            
        }
    }
}