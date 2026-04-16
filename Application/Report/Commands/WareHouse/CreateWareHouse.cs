using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.WareHouse
{
    public class CreateWareHouse
    {
        public class Command : IRequest<Result<WareHouseResponseDto>>
        {
            public required WareHouseCreateDto wareHouseCreateDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.wareHouseCreateDto).NotNull().WithMessage("Empty Data");

                When(x => x.wareHouseCreateDto != null, () =>
                {
                    RuleFor(x => x.wareHouseCreateDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.wareHouseCreateDto.description).NotEmpty().WithMessage("Description is required");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<WareHouseResponseDto>>
        {
            public async Task<Result<WareHouseResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = mapper.Map<Domain.WareHouse>(request.wareHouseCreateDto);

                context.WareHouses.Add(wareHouse);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<WareHouseResponseDto>.Failure("Failed to create warehouse", 500);

                return Result<WareHouseResponseDto>.Success(mapper.Map<WareHouseResponseDto>(wareHouse));
            }
        }
    }
}