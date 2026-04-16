using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Report.Commands.WareHouse
{
    public static class WareHouseValidationRules
    {
        public static IRuleBuilderOptions<T, Guid> MustHaveValidCity<T>(
            this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.Cities.AnyAsync(c => c.Id == id, ct))
                       .WithMessage("City not found");

        public static IRuleBuilderOptions<T, Guid> MustHaveValidWareHouse<T>(
            this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.WareHouses.AnyAsync(w => w.Id == id, ct))
                       .WithMessage("WareHouse not found");
    }
}