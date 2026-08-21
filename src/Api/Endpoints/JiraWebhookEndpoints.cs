using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
    [ApiController]
    [Route("webhooks")]
    public class JiraWebhookController : ControllerBase
    {
        private readonly IIncidentIntakeService _incidentIntakeService;

        public JiraWebhookController(
            IIncidentIntakeService incidentIntakeService)
        {
            _incidentIntakeService = incidentIntakeService;
        }

        [HttpPost("jira")]
        public async Task<IActionResult> ReceiveJiraWebhook()
        {
            using var reader = new StreamReader(Request.Body);

            var rawPayload = await reader.ReadToEndAsync();

            await _incidentIntakeService.ProcessAsync(
                rawPayload);

            return Accepted();
        }

        [HttpGet("Health")]
        public IActionResult Check()
        {
            return Ok(new
            {
                status = "API is working"
            });
        }
    }
}
