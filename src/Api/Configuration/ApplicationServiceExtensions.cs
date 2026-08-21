using Application.Interfaces;
using Application.UseCases;

using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddScoped<IIncidentIntakeService, IncidentIntakeService>();

            return services;
        }
    }
}
