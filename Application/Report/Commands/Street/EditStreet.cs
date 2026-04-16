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
    public class EditStreet
    {
        public class Command : IRequest<Result<StreetResponseDto>>
        {
            public required StreetEditDto StreetEditDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.StreetEditDto).NotNull().WithMessage("Empty Data");

                When(x => x.StreetEditDto != null, () =>
                {
                    RuleFor(x => x.StreetEditDto.Id).MustHaveValidStreet(context);
                    RuleFor(x => x.StreetEditDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.StreetEditDto.description).NotEmpty().WithMessage("Description is required");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<StreetResponseDto>>
        {
            public async Task<Result<StreetResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = await context.Streets.FindAsync([request.StreetEditDto.Id], cancellationToken);

                mapper.Map(request.StreetEditDto, street);

                await context.SaveChangesAsync(cancellationToken);

                return Result<StreetResponseDto>.Success(mapper.Map<StreetResponseDto>(street));
            }
        }
    }
}