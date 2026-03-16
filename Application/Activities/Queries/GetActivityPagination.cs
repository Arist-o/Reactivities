using Application.Activities.DTOs;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.Activities.Queries
{

    public class GetActivityPagination
    {
    
        public class Query : IRequest<Result<PagedResult<ActivityDto>>>
        {


            [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
            public int PageNumber { get; set; } = 1;
            [Range(1,50, ErrorMessage = "Page size must be beetween 1 and 50")]
            public int PageSize { get; set; } = 10;
         
        }
        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<PagedResult<ActivityDto>>>
        {
            public async Task<Result<PagedResult<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Activities.AsNoTracking();

                var totalCount = await query.CountAsync(cancellationToken);
               var items = await query
                    .OrderBy(x => x.Id)
                    .Skip((request.PageNumber - 1  ) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                var dtos = mapper.Map<IEnumerable<ActivityDto>>(items);

                var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

                var metadata = new PaginationMetadata
                {
                    TotalCount = totalCount,
                    PageSize = request.PageSize,
                    CurrentPage = request.PageNumber,
                    TotalPages = totalPages,
                    HasNext = request.PageNumber < totalPages,
                    HasPrevious = request.PageNumber > 1
                };

                var pagedResult = new PagedResult<ActivityDto>
                {
                    Data = dtos,
                    Metadata = metadata
                };
                return Result<PagedResult<ActivityDto>>.Success(pagedResult);
            }
        }

        
    }
   
}
