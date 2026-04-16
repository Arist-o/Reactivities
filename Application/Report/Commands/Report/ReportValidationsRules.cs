using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace Application.Report.Commands.Report
{
    public static class ReportValidationRules
    {
        public static IRuleBuilderOptions<T, Guid> MustHaveValidArea<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.Areas.AnyAsync(a => a.Id == id, ct)).WithMessage("Area not found");

        public static IRuleBuilderOptions<T, Guid> MustHaveValidCity<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.Cities.AnyAsync(c => c.Id == id, ct)).WithMessage("City not found");

        public static IRuleBuilderOptions<T, Guid> MustHaveValidStreet<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.Streets.AnyAsync(s => s.Id == id, ct)).WithMessage("Street not found");

        public static IRuleBuilderOptions<T, Guid> MustHaveValidWareHouse<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.WareHouses.AnyAsync(w => w.Id == id, ct)).WithMessage("WareHouse not found");

        public static IRuleBuilderOptions<T, Guid> MustHaveValidReport<T>(this IRuleBuilder<T, Guid> ruleBuilder, IAppDbContext context) =>
            ruleBuilder.MustAsync(async (id, ct) => await context.Reports.AnyAsync(r => r.Id == id, ct)).WithMessage("Report not found");
    }
}