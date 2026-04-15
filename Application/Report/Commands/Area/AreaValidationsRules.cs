using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Report.Commands.Area
{
    public static class AreaValidationsRules
    {
        public static IRuleBuilderOptions<T, Guid?> MustHaveValidCity<T>(
            this IRuleBuilder<T, Guid?> ruleBuilder, IAppDbContext context)
        {
            return ruleBuilder.MustAsync(async (cityId, ct) =>
                cityId == null || await context.Cities.AnyAsync(c => c.Id == cityId, ct))
                .WithMessage("Вказаного міста (AreaCenterId) не існує.");
        }

    }
}
