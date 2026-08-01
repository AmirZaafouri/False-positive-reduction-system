using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common
{
    public class IncidentIntake
    {
        public required string TicketId { get; init; }
        public required string Summary { get; init; }
        public string? Status { get; init; }
        public string? IssueType { get; init; }
        public string? Environment { get; init; }
        public DateTimeOffset? CreatedAt { get; init; }
        public required string SourceProvider { get; init; }
        public required string RawPayload { get; init; }
        public DateTimeOffset ReceivedAt { get; init; } = DateTimeOffset.UtcNow;


    }
}
