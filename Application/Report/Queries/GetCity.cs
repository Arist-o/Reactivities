using Application.Core;
using Application.Report.DTOs.City;
using AutoMapper;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Application.Report.Queries
{
    public class GetCity
    {
        public class Query : IRequest<Result<List<CityResponseDto>>>
        {
        }

        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Query, Result<List<CityResponseDto>>>
        {

            public async Task<Result<List<CityResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var citys = await context.Cities
                    .ProjectTo<CityResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
           
                return Result<List<CityResponseDto>>.Success(citys);
            }

        }
    }
}

