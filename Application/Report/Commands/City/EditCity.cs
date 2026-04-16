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
    public class EditCity
    {
        public class Command : IRequest<Result<CityResponseDto>>
        {
            public required CityEditDto CityEditDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.CityEditDto)
                 .NotNull()
                 .WithMessage("Empty Data");

                When(x => x.CityEditDto != null, () =>
                {
                    RuleFor(x => x.CityEditDto.Id)
                        .MustAsync(async (id, ct) => await context.Cities.AnyAsync(c => c.Id == id, ct))
                        .WithMessage("City not found");

                    RuleFor(x => x.CityEditDto.Description)
                        .NotEmpty().WithMessage("Description is required");

                    RuleFor(x => x.CityEditDto.AreaId)
                        .MustAsync(async (areaId, ct) => await context.Areas.AnyAsync(a => a.Id == areaId, ct))
                        .WithMessage("Area not found");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<CityResponseDto>>
        {
            public async Task<Result<CityResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var city = await context.Cities.FindAsync([request.CityEditDto.Id], cancellationToken);

                mapper.Map(request.CityEditDto, city);

                await context.SaveChangesAsync(cancellationToken);


                return Result<CityResponseDto>.Success(mapper.Map<CityResponseDto>(city));
            }
        }
    }
}