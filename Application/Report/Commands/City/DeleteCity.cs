using Application.Core;
using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.City
{
    public class DeleteCity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([ request.Id ], cancellationToken);
                if (city == null) return Result<Unit>.Failure("City not found", 404);
                context.Cities.Remove(city);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete city", 400);
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
