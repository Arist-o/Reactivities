using Application.Core;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.WareHouse
{
    public class DeleteWareHouse
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }
        public class Handler(AppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = await context.WareHouses.FindAsync(request.Id);
                
                if (wareHouse == null) return Result<Unit>.Failure("WareHouse not found", 404);

                context.WareHouses.Remove(wareHouse);

                var result = await context.SaveChangesAsync() > 0;

                if(!result) return Result<Unit>.Failure("Failed to delete wareHouse", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
