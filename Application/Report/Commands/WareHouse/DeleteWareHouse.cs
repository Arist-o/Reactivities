using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.WareHouse
{
    public class DeleteWareHouse
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.Id).MustHaveValidWareHouse(context);
            }
        }

        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = await context.WareHouses.FindAsync([request.Id], cancellationToken);

                context.WareHouses.Remove(wareHouse!);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete warehouse", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}