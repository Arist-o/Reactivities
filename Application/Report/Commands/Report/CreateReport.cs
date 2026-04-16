using Application.Core;
using Application.Interfaces;
using Application.Report.DTOs.Report;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Report.Commands.Report
{
    public class CreateReport
    {
        public class Command : IRequest<Result<ReportResponseDto>>
        {
            public required ReportCreateDto reportCreateDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.reportCreateDto).NotNull().WithMessage("Empty Data");

                When(x => x.reportCreateDto != null, () =>
                {
                    RuleFor(x => x.reportCreateDto.AreaId).MustHaveValidArea(context);
                    RuleFor(x => x.reportCreateDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.reportCreateDto.StreetId).MustHaveValidStreet(context);
                    RuleFor(x => x.reportCreateDto.WareHouseId).MustHaveValidWareHouse(context);

                    RuleFor(x => x.reportCreateDto.email).NotEmpty().EmailAddress();
                    RuleFor(x => x.reportCreateDto.phone).NotEmpty();
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<ReportResponseDto>>
        {
            public async Task<Result<ReportResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var report = mapper.Map<Domain.Report>(request.reportCreateDto);

                context.Reports.Add(report);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<ReportResponseDto>.Failure("Failed to create report", 500);

                return Result<ReportResponseDto>.Success(mapper.Map<ReportResponseDto>(report));
            }
        }
    }
}