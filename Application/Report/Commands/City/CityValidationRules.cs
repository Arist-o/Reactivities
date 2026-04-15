using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Report.Commands.City
{
    public static class CityValidationRules
    {
        public static IRuleBuilderOptions<T, Guid> MustHaveValidArea<T>(
           this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context)
        {
            return ruleBuilder.MustAsync(async (areaId, ct) =>
                await context.Areas.AnyAsync(a => a.Id == areaId, ct))
                .WithMessage("Вказаної області (AreaId) не існує.");
        }
    }
}
