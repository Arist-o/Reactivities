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
    public class EditWareHouse
    {
        public class Command : IRequest<Result<WareHouseResponseDto>>
        {
            public required WareHouseEditDto WareHouseEditDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.WareHouseEditDto).NotNull().WithMessage("Empty Data");

                When(x => x.WareHouseEditDto != null, () =>
                {
                    RuleFor(x => x.WareHouseEditDto.Id).MustHaveValidWareHouse(context);
                    RuleFor(x => x.WareHouseEditDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.WareHouseEditDto.description).NotEmpty().WithMessage("Description is required");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<WareHouseResponseDto>>
        {
            public async Task<Result<WareHouseResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = await context.WareHouses.FindAsync([request.WareHouseEditDto.Id], cancellationToken);

                mapper.Map(request.WareHouseEditDto, wareHouse);

                await context.SaveChangesAsync(cancellationToken);

                return Result<WareHouseResponseDto>.Success(mapper.Map<WareHouseResponseDto>(wareHouse));
            }
        }
    }
}