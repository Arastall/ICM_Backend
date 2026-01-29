using ICMServer.Helpers;
using ICMServer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ICMServer.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCalculationServices(this IServiceCollection services)
        {
            // Register helpers
            services.AddScoped<ISystemParametersHelper, SystemParametersHelper>();

            // Register services
            services.AddScoped<IOrderPreparationService, OrderPreparationService>();
            services.AddScoped<IProductValidationService, ProductValidationService>();
            services.AddScoped<IPerformanceService, PerformanceService>();
            services.AddScoped<IRevenueCalculationService, RevenueCalculationService>();
            services.AddScoped<IRollupAllocationService, RollupAllocationService>();
            services.AddScoped<ICreditAllocationService, CreditAllocationService>();
            //services.AddScoped<IRevenueTotalsService, RevenueTotalsService>();
            services.AddScoped<ICommissionPaymentService, CommissionPaymentService>();
            services.AddScoped<ICalculationService, CalculationService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IPeriodContext, PeriodContextService>();

            return services;
        }
    }
}