using Application.Activities.DTOs;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;


namespace Application.Activities.Queries
{
    public class GetActivityMultipleSearchDetails
    {
        public class Query : IRequest<Result<Application.Core.PagedResult<ActivityDto>>>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public string? Search { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
            public int PageNumber { get; set; } = 1;

            [Range(1, 50, ErrorMessage = "Page size must be beetween 1 and 50")]
            public int PageSize { get; set; } = 10;

            public string[]? ids { get; set; } 

            public string? ColumnName { get; set; }

            public bool AscDesc { get; set; } = true;
        }

        public class Handler(AppDbContext context, IMapper mapper,IUserAccessor userAccessor) : IRequestHandler<Query, Result<Application.Core.PagedResult<ActivityDto>>>
        {
            public async Task<Result<Application.Core.PagedResult<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                if (request.StartDate != default && request.EndDate != default && request.StartDate > request.EndDate)
                {
                    return Result<Application.Core.PagedResult<ActivityDto>>.Failure("Start date cannot be after end date", 400);
                }

                if (request.EndDate != default)
                {
                    request.EndDate = request.EndDate.Date.AddDays(1).AddTicks(-1);
                }


                var query = context.Activities.AsNoTracking();
            

                if (request.StartDate != default)
                {
                    query = query.Where(x => x.Date >= request.StartDate);
                }

                if (request.EndDate != default)
                {
                    query = query.Where(x => x.Date <= request.EndDate);
                }

                if (!string.IsNullOrWhiteSpace(request.Search))
                {
                    query = query.Where(x => x.Title.Contains(request.Search)
                                          || x.Description.Contains(request.Search)
                                          || x.City.Contains(request.Search)
                                          || x.Venue.Contains(request.Search));
                }
                if (request.ids != null && request.ids.Any())
                {
                    query = query.Where(activity =>
                        activity.Attendees.Any(attendee => request.ids.Contains(attendee.UserId)));
                }
                var totalCount = await query.CountAsync(cancellationToken);
               
                if (totalCount == 0)
                {
                    return Result<Application.Core.PagedResult<ActivityDto>>.Failure("Activities not found", 404);
                }


                if (!string.IsNullOrWhiteSpace(request.ColumnName))
                {
                    string sortOrder = request.AscDesc ? "ascending" : "descending";

                    query = query.OrderBy($"{request.ColumnName} {sortOrder}");
                }
                else 
                {
                    query = query.OrderBy(x => x.Title);
                }


                var currentUserId = userAccessor.GetUserId();

                
                var dtos = await query
                    .ProjectTo<ActivityDto>(mapper.ConfigurationProvider, new { currentUserId })
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);





                
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

                var pagedResult = new Application.Core.PagedResult<ActivityDto>
                {
                    Data = dtos,
                    Metadata = metadata
                };

                return Result<Application.Core.PagedResult<ActivityDto>>.Success(pagedResult);
            }
        }
    }
}
