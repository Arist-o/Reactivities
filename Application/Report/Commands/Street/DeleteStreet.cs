using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.Street
{
    public class DeleteStreet
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.Id).MustHaveValidStreet(context);
            }
        }

        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = await context.Streets.FindAsync([request.Id], cancellationToken);

                context.Streets.Remove(street!);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete street", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}