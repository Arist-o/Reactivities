using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Profiles.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Profiles.Queries
{
    public class GetProfile
    {
        public class Query : IRequest<Result<UserProfile>>
        {
            public required string UserId { get; set; }
        }

        public class Handler(IAppDbContext context, IMapper mapper, IUserAccessor userAccessor) : IRequestHandler<Query, Result<UserProfile>>
        {
            public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profile = await context.Users
                    .AsNoTracking() 
                    .ProjectTo<UserProfile>(mapper.ConfigurationProvider,
                        new { currentUserId = userAccessor.GetUserId() })
                    .SingleOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                return profile == null
                    ? Result<UserProfile>.Failure("Profile not found", 404)
                    : Result<UserProfile>.Success(profile);
            }
        }
    }
}