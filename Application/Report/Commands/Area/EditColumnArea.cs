using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Area;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.Area
{
    public class EditColumnArea
    {
        public class Command : IRequest<Result<AreaResponseDto>>
        {
            public required AreaEditColumnDto AreaEditColumnDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.AreaEditColumnDto)
                    .NotNull()
                    .WithMessage("Empty Data");

                When(x => x.AreaEditColumnDto != null, () =>
                {
                    RuleFor(x => x.AreaEditColumnDto.Id)
                        .MustAsync(async (id, ct) => await context.Areas.AnyAsync(a => a.Id == id, ct))
                        .WithMessage("Area not found");

                    RuleFor(x => x.AreaEditColumnDto.AreaCenterId)
                        .MustAsync(async (cityId, ct) => await context.Cities.AnyAsync(c => c.Id == cityId, ct))
                        .WithMessage("City not found");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<AreaResponseDto>>
        {
            public async Task<Result<AreaResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = await context.Areas.FindAsync([request.AreaEditColumnDto.Id], cancellationToken);

                area!.AreaCenterId = request.AreaEditColumnDto.AreaCenterId;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<AreaResponseDto>.Failure("Failed to update area column", 500);

                return Result<AreaResponseDto>.Success(mapper.Map<AreaResponseDto>(area));
            }
        }
    }
}