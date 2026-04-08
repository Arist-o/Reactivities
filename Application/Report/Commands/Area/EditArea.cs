using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Area;
using AutoMapper;
using MediatR;

using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.Area
{
    public class EditArea
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required AreaEditDto AreaEditDto { get; set; }
        }
        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = await context.Areas.FindAsync([request.AreaEditDto.Id], cancellationToken);
                if (area == null) return Result<Unit>.Failure("Area not found", 404);
                mapper.Map(request.AreaEditDto, area);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update area", 400);
                return Result<Unit>.Success(Unit.Value);
            }
        }
       }
}
