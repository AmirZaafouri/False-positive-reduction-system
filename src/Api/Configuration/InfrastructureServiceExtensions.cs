using Application.Interfaces;
using Infrastructure.Ticketing.Jira;

namespace Api.Configuration
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ITicketPayloadParser, JiraPayloadParser>();

            return services;
        }
    }
}
