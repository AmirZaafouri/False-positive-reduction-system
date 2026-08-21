namespace Domain.Entities
{
    /// <summary>
    /// Represents a failed-test incident received from a ticketing system.
    /// This is the core aggregate that flows through the validation pipeline.
    /// </summary>
    public class Incident
    {
        public Guid Id { get; private set; }

        /// <summary>Ticket identifier from the originating system (e.g. "SCRUM-8").</summary>
        public string TicketId { get; private set; } = string.Empty;

        /// <summary>One-line description of the failure.</summary>
        public string Summary { get; private set; } = string.Empty;

        /// <summary>Current ticket status as reported by the source system.</summary>
        public string? Status { get; private set; }

        /// <summary>Issue type as reported by the source system (e.g. "Bug").</summary>
        public string? IssueType { get; private set; }

        /// <summary>Target environment where the failure occurred (e.g. "REGALI").</summary>
        public string? Environment { get; private set; }

        /// <summary>When the ticket was originally created in the source system.</summary>
        public DateTimeOffset? CreatedAt { get; private set; }

        /// <summary>Source ticketing provider (e.g. "Jira", "AzureDevOps").</summary>
        public string SourceProvider { get; private set; } = string.Empty;

        /// <summary>Unmodified raw payload as received from the provider.</summary>
        public string RawPayload { get; private set; } = string.Empty;

        /// <summary>When this incident was received by the engine.</summary>
        public DateTimeOffset ReceivedAt { get; private set; }

        // Private constructor for ORM / rehydration
        private Incident() { }

        public static Incident Create(
            string ticketId,
            string summary,
            string? status,
            string? issueType,
            string? environment,
            DateTimeOffset? createdAt,
            string sourceProvider,
            string rawPayload)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(ticketId);
            ArgumentException.ThrowIfNullOrWhiteSpace(summary);
            ArgumentException.ThrowIfNullOrWhiteSpace(sourceProvider);
            ArgumentException.ThrowIfNullOrWhiteSpace(rawPayload);

            return new Incident
            {
                Id = Guid.NewGuid(),
                TicketId = ticketId,
                Summary = summary,
                Status = status,
                IssueType = issueType,
                Environment = environment,
                CreatedAt = createdAt,
                SourceProvider = sourceProvider,
                RawPayload = rawPayload,
                ReceivedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
