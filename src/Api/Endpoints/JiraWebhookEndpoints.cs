using Application.Interfaces;
using Application.UseCases;


namespace Api.Endpoints
{
    public static class JiraWebhookEndpoints
    {
        public static void MapJiraWebhookEndpoints(this WebApplication app)
        {
            app.MapPost("/webhooks/jira", async (
                HttpRequest request,
                IIncidentIntakeService incidentIntakeService) =>
            {
                using var reader = new StreamReader(request.Body);

                var rawPayload = await reader.ReadToEndAsync();

                await incidentIntakeService.ProcessAsync(rawPayload);

                return Results.Accepted();
            })
            .WithName("ReceiveJiraWebhook");
        }
    }
}
