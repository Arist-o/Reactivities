using Application.Core;
using Application.Report.DTOs.City;
using Application.Report.DTOs.Street;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Street
{
    public class CreateStreet
    {
        public class Command : IRequest<Result<Guid>>
        { 
            public required StreetCreateDto streetCreateDto { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command,Result<Guid>>
        { 
            public async Task<Result<Guid>> Handle(Command request,CancellationToken cancellationToken)
            {
                var street = mapper.Map<Domain.Street>(request.streetCreateDto);

                context.Streets.Add(street);

                var result = await  context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Guid>.Failure("Failed to create street",400);

                return Result<Guid>.Success(street.Id);
            }
        }
    }
}
