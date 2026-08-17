using Application.Common;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;


namespace Application.UseCases
{
    public sealed class IncidentIntakeService : IIncidentIntakeService
    {
        private readonly ITicketPayloadParser _parser;
        private readonly ILogger<IncidentIntakeService> _logger;

        public IncidentIntakeService(
            ITicketPayloadParser parser,
            ILogger<IncidentIntakeService> logger)
        {
            _parser = parser;
            _logger = logger;
        }

        public string ProviderName => throw new NotImplementedException();

        public Task ProcessAsync(string rawPayload)
        {
            var incident = _parser.Parse(rawPayload);

            _logger.LogInformation(
                "Parsed incident intake: TicketId={TicketId}, Summary={Summary}, Status={Status}, IssueType={IssueType}, Provider={Provider}",
                incident.TicketId,
                incident.Summary,
                incident.Status,
                incident.IssueType,
                incident.SourceProvider);

            return Task.CompletedTask;
        }
    }
}
