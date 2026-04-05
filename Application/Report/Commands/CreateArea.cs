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
        public class Command : IRequest<Result<string>>
        {
            public required AreaCreateDto AreaCreateDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var area = mapper.Map<Domain.Area>(request.AreaCreateDto);
                context.Areas.Add(area);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<string>.Failure("Failed to create area", 500);
                return Result<string>.Success(area.Id);
            }
        }
    }
}

