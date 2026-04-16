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
    public class EditColumnWareHouse
    {
        public class Command : IRequest<Result<WareHouseResponseDto>>
        {
            public required WareHouseEditColumnDto WareHouseEditColumnDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.WareHouseEditColumnDto).NotNull().WithMessage("Empty Data");

                When(x => x.WareHouseEditColumnDto != null, () =>
                {
                    RuleFor(x => x.WareHouseEditColumnDto.Id).MustHaveValidWareHouse(context);
                    RuleFor(x => x.WareHouseEditColumnDto.CityId).MustHaveValidCity(context);
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<WareHouseResponseDto>>
        {
            public async Task<Result<WareHouseResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = await context.WareHouses.FindAsync([request.WareHouseEditColumnDto.Id], cancellationToken);

                wareHouse!.CityId = request.WareHouseEditColumnDto.CityId;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<WareHouseResponseDto>.Failure("Failed to update warehouse column", 500);

                return Result<WareHouseResponseDto>.Success(mapper.Map<WareHouseResponseDto>(wareHouse));
            }
        }
    }
}