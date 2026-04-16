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
    public class EditArea
    {
        public class Command : IRequest<Result<AreaResponseDto>>
        {
            public required AreaEditDto AreaEditDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.AreaEditDto)
                 .NotNull()
                 .WithMessage("Empty Data");

                When(x => x.AreaEditDto != null, () =>
                {
                    RuleFor(x => x.AreaEditDto.Id)
                        .MustAsync(async (id, ct) => await context.Areas.AnyAsync(a => a.Id == id, ct))
                        .WithMessage("Area not found");

                    RuleFor(x => x.AreaEditDto.Description)
                        .NotEmpty().WithMessage("Description is required");

                    RuleFor(x => x.AreaEditDto.AreaCenterId)
                        .MustAsync(async (cityId, ct) => await context.Cities.AnyAsync(c => c.Id == cityId, ct))
                        .WithMessage("City not found");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<AreaResponseDto>>
        {
            public async Task<Result<AreaResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = await context.Areas.FindAsync([request.AreaEditDto.Id], cancellationToken);

                mapper.Map(request.AreaEditDto, area);

                await context.SaveChangesAsync(cancellationToken);

                

                return Result<AreaResponseDto>.Success(mapper.Map<AreaResponseDto>(area));
            }
        }
    }
}