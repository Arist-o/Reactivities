using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Street;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Street
{
    public class EditStreet
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required StreetEditDto StreetEditDto { get; set; }
        }   
        public class Handler(IAppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var street =  context.Streets.FindAsync([request.StreetEditDto.Id], cancellationToken).Result;
                if(street == null) return Result<Unit>.Failure("Street not found", 404);

                mapper.Map(request.StreetEditDto, street);

                var result = await context.SaveChangesAsync(cancellationToken) > 0; 

                if(!result) return Result<Unit>.Failure("Failed to update street", 400);

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
