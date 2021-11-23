using System;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Financial
{
    public static class Configuration
    {
        public static IServiceCollection AddFinancialServices(this IServiceCollection services) => services
            .AddFinancialDataAccess()
            .AddScoped<IFinancialTransactionService, FinancialTransactionService>()
            .AddScoped<IValidator<FinancialTransaction>, FinancialTransactionValidator>()
            .AddScoped<IValidator<FinancialTransactionAttribute>, FinancialTransactionAttributeValidator>();
    }
}