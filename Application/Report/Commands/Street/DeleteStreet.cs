using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Street
{
    public class DeleteStreet
    {
        public class Command : IRequest<Result<Unit>>
        { 
            public required Guid Id { get; set; }
        }

        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        { 
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = await context.Streets.FindAsync([request.Id ], cancellationToken);
                if (street == null) return Result<Unit>.Failure("Street not found", 404);

                context.Streets.Remove(street);

                var result = await context.SaveChangesAsync(cancellationToken) > 0; 
                if(!result) return Result<Unit>.Failure("Failed to delete street", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
