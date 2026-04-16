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
    public class CreateStreet
    {
        public class Command : IRequest<Result<StreetResponseDto>>
        {
            public required StreetCreateDto streetCreateDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.streetCreateDto).NotNull().WithMessage("Empty Data");

                When(x => x.streetCreateDto != null, () =>
                {
                    RuleFor(x => x.streetCreateDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.streetCreateDto.description).NotEmpty().WithMessage("Description is required");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<StreetResponseDto>>
        {
            public async Task<Result<StreetResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var street = mapper.Map<Domain.Street>(request.streetCreateDto);

                context.Streets.Add(street);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<StreetResponseDto>.Failure("Failed to create street", 500);

                return Result<StreetResponseDto>.Success(mapper.Map<StreetResponseDto>(street));
            }
        }
    }
}