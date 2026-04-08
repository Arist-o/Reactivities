using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Street;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Queries
{
    public class GetStreet
    {
        public class Query : IRequest<Result<List<StreetResponseDto>>>
        {
            
        }
        public class Handler(IAppDbContext context,IMapper mapper) : IRequestHandler<Query, Result<List<StreetResponseDto>>>
        {
            public async Task<Result<List<StreetResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var streets = await context.Streets
                    .ProjectTo<StreetResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return  Result<List<StreetResponseDto>>.Success(streets);
            }
        }
    }
}
