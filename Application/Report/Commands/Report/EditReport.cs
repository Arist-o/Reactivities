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
    public class EditReport
    {
        public class Command : IRequest<Result<ReportResponseDto>>
        {
            public required ReportEditDto ReportEditDto { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IAppDbContext context)
            {
                RuleFor(x => x.ReportEditDto).NotNull().WithMessage("Empty Data");

                When(x => x.ReportEditDto != null, () =>
                {
                    RuleFor(x => x.ReportEditDto.Id).MustHaveValidReport(context);
                    RuleFor(x => x.ReportEditDto.AreaId).MustHaveValidArea(context);
                    RuleFor(x => x.ReportEditDto.CityId).MustHaveValidCity(context);
                    RuleFor(x => x.ReportEditDto.StreetId).MustHaveValidStreet(context);
                    RuleFor(x => x.ReportEditDto.WareHouseId).MustHaveValidWareHouse(context);

                    RuleFor(x => x.ReportEditDto.email).NotEmpty().EmailAddress();
                    RuleFor(x => x.ReportEditDto.phone).NotEmpty();
                });
            }
        }

        public class Handler(IAppDbContext context, IMapper mapper) : IRequestHandler<Command, Result<ReportResponseDto>>
        {
            public async Task<Result<ReportResponseDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var report = await context.Reports.FindAsync([request.ReportEditDto.Id], cancellationToken);

                mapper.Map(request.ReportEditDto, report);

                await context.SaveChangesAsync(cancellationToken);

                return Result<ReportResponseDto>.Success(mapper.Map<ReportResponseDto>(report));
            }
        }
    }
}