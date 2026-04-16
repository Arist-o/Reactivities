using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Report
{
    public class DeleteReport
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required Guid Id { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.Id).MustHaveValidReport(context);
            }
        }
        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var report = await context.Reports.FindAsync([request.Id], cancellationToken);

                context.Reports.Remove(report!);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete report", 400);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
