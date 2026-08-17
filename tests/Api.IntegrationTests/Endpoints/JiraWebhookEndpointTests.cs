using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Api.IntegrationTests.Endpoints
{
    public class JiraWebhookEndpointTests
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public JiraWebhookEndpointTests(
            WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task PostJiraWebhook_ValidPayload_ReturnsAccepted()
        {
            var payload = new
            {
                ticketId = "SCRUM-8",
                summary = "Robot test failed",
                status = "To Do",
                issueType = "Bug",
                environment = "REGALI",
                createdAt = "2026-08-17T20:00:00Z"
            };

            using var response = await _client.PostAsJsonAsync(
                "/webhooks/jira",
                payload);

            Assert.Equal(
                HttpStatusCode.Accepted,
                response.StatusCode);
        }
    }
}
