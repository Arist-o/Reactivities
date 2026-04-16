using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.City
{
    public class DeleteCity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.Id)
                    .MustAsync(async (id, ct) => await context.Cities.AnyAsync(c => c.Id == id, ct))
                    .WithMessage("City not found");
            }
        }

        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([request.Id], cancellationToken);

                context.Cities.Remove(city!);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete city", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}