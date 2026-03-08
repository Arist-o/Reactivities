using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using Persistence;
using Application.Activities.DTOs;
using AutoMapper;
using FluentValidation;
using Application.Core;
namespace Application.Activities.Commands
{
    public class CreateActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required CreateActivityDto ActivityDto { get; set; }
        }

        public class Handler(AppDbContext context, IMapper mapper,IValidator<Command> validator) : IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = mapper.Map<Activity>(request.ActivityDto);

                context.Activities.Add(activity);

               var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<string>.Failure("Failed to create activity", 500);
                return Result<string>.Success(activity.Id);
            }
        }
    }
}
