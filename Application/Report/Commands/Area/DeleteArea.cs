using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Area
{
    public class DeleteArea
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }
        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
               var area = await context.Areas.FindAsync([request.Id], cancellationToken);   

                if (area == null) return Result<Unit>.Failure("Area not found", 404);

                context.Areas.Remove(area);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if(!result) return Result<Unit>.Failure("Failed to delete area", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
