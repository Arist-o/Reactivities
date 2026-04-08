using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.WareHouse;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Report.Commands.WareHouse
{
    public class CreateWareHouse
    {
        public class Command : IRequest<Result<Guid>>
        {
            public required WareHouseCreateDto wareHouseCreateDto { get; set; }
        }
        public class Handler(IAppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Guid>>
        {
            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var wareHouse = mapper.Map<Domain.WareHouse>(request.wareHouseCreateDto);

                context.WareHouses.Add(wareHouse);

                var result = await context.SaveChangesAsync(cancellationToken) > 0; 

                if(!result) return Result<Guid>.Failure("Failed to create ware house", 400);

                return Result<Guid>.Success(wareHouse.Id);
            }
        }
    }
}
