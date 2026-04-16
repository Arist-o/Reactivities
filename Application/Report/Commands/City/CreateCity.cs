using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.City;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.City
{
    public class CreateCity
    {
        public class Command : IRequest<Result<CityResponseDto>>
        {
            public required CityCreateDto CityCreateDto { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {

            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.CityCreateDto)
                    .NotNull()
                    .WithMessage("Empty Data");

                When(x => x.CityCreateDto != null, () =>
                {
                    RuleFor(x => x.CityCreateDto.AreaId)
                        .NotEmpty().WithMessage("AreaId is required")
                        .MustHaveValidArea(context);

                    RuleFor(x => x.CityCreateDto.Description)
                        .NotEmpty().WithMessage("Description is required");
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<CityResponseDto>>
        {
            
            public async Task<Result<CityResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            { 
                var city = mapper.Map<Domain.City>(request.CityCreateDto);
                context.Cities.Add(city);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<CityResponseDto>.Failure("Failed to create city", 500);
                return Result<CityResponseDto>.Success(mapper.Map<CityResponseDto>(city));
            }
        }
    }
}
