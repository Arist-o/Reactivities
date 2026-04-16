using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Street;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.Street
{
    public class EditColumnStreet
    {
        public class Command : IRequest<Result<StreetResponseDto>>
        {
            public required StreetEditColumnDto StreetEditColumnDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.StreetEditColumnDto).NotNull().WithMessage("Empty Data");

                When(x => x.StreetEditColumnDto != null, () =>
                {
                    RuleFor(x => x.StreetEditColumnDto.Id).MustHaveValidStreet(context);
                    RuleFor(x => x.StreetEditColumnDto.CityId).MustHaveValidCity(context);
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<StreetResponseDto>>
        {
            public async Task<Result<StreetResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = await context.Streets.FindAsync([request.StreetEditColumnDto.Id], cancellationToken);

                street!.CityId = request.StreetEditColumnDto.CityId;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<StreetResponseDto>.Failure("Failed to update street column", 500);

                return Result<StreetResponseDto>.Success(mapper.Map<StreetResponseDto>(street));
            }
        }
    }
}