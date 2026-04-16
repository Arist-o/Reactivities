using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.City;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.City
{
    public class EditColumnCity
    {
        public class Command : IRequest<Result<CityResponseDto>>
        {
            public required CityEditColumnDto CityEditColumnDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.CityEditColumnDto)
                    .NotNull()
                    .WithMessage("Empty Data");

                When(x => x.CityEditColumnDto != null, () =>
                {
                    RuleFor(x => x.CityEditColumnDto.Id)
                        .MustAsync(async (id, ct) => await context.Cities.AnyAsync(c => c.Id == id, ct))
                        .WithMessage("City not found");

                    RuleFor(x => x.CityEditColumnDto.AreaId)
                        .MustAsync(async (areaId, ct) => await context.Areas.AnyAsync(a => a.Id == areaId, ct))
                        .WithMessage("Area not found");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<CityResponseDto>>
        {
            public async Task<Result<CityResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([request.CityEditColumnDto.Id], cancellationToken);

                city!.AreaId = request.CityEditColumnDto.AreaId;

                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<CityResponseDto>.Failure("Failed to update city column", 500);

                return Result<CityResponseDto>.Success(mapper.Map<CityResponseDto>(city));
            }
        }
    }
}