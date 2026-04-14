using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore; // Для базових методів        // Для Bulk-методів
namespace Application.Report.Commands.Area
{
    public class CreateAreasBulk
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required List<CreateAreaWithCenter.Command> Items { get; set; }
        }

        public class Handler(IAppDbContext context, IMapper mapper)
     : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken ct)
            {
                var areas = new List<Domain.Area>();
                var cities = new List<Domain.City>();

                foreach (var item in request.Items)
                {
                    areas.Add(mapper.Map<Domain.Area>(item.Area));
                    cities.Add(mapper.Map<Domain.City>(item.City));
                }

                await context.BulkCreateAreasWithCentersAsync(areas, cities, ct);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}