using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Area;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Area
{
    public class CreateArea
    {
        public class Command : IRequest<Result<AreaResponseDto>>
        {
            public required AreaCreateDto AreaCreateDto { get; set; }
        }
        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.AreaCreateDto)
                 .NotNull()
                 .WithMessage("Empty Data");

                When(x => x.AreaCreateDto != null, () =>
                {


                    RuleFor(x => x.AreaCreateDto.Description)
                        .NotEmpty().WithMessage("Description is required");
                });


            }
        }
        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<AreaResponseDto>>
        {
            public async Task<Result<AreaResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            { 
                var area = mapper.Map<Domain.Area>(request.AreaCreateDto);
                context.Areas.Add(area);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<AreaResponseDto>.Failure("Failed to create area", 500);
                return Result<AreaResponseDto>.Success(mapper.Map<AreaResponseDto>(area));
            }
        }
    }
}

