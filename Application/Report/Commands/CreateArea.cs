using Application.Core;
using Application.Report.DTOs;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands
{
    public class CreateArea
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required AreaCreateDto AreaCreateDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = mapper.Map<Domain.Area>(request.AreaCreateDto);
                context.Areas.Add(area);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Guid>.Failure("Failed to create area", 500);
                return Result<Guid>.Success(area.Id);
            }
        }
    }
}

