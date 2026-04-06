using Application.Core;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Queries
{
    public class GetWareHouse
    {
        public class Query : IRequest<Result<List<WareHouseResponseDto>>>
        {

        }

        public class Handler(AppDbContext context, IMapper mapper) : IRequestHandler<Query, Result<List<WareHouseResponseDto>>>
        {
            public async Task<Result<List<WareHouseResponseDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                   var wareHouses = await context.WareHouses
                    .ProjectTo<WareHouseResponseDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return Result<List<WareHouseResponseDto>>.Success(wareHouses);
            }
        }
    }
}
