using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Application.Interfaces;

namespace Application.Profiles.Queries
{
    public class GetComments
    {
        public class Query : IRequest<Result<List<CommentDto>>>
        {
            public required string ActivityId { get; set; }
        }

        public class Handler(IAppDbContext context, IMapper mapper)
            : IRequestHandler<Query, Result<List<CommentDto>>>
        {
            public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
               var comments = await context.Comments
                    .Where(x => x.ActivityId == request.ActivityId)
                    .OrderByDescending(x => x.CreatedAt)
                    .ProjectTo<CommentDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
                return Result<List<CommentDto>>.Success(comments);
            }
        }
    }
}
