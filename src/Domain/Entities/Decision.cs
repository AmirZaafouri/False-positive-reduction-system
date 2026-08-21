using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// Represents the outcome produced by the Decision Engine for a given Incident.
    /// </summary>
    public class Decision
    {
        public Guid Id { get; private set; }

        /// <summary>The incident this decision was made for.</summary>
        public Guid IncidentId { get; private set; }

        /// <summary>The classification outcome.</summary>
        public IncidentClassification Classification { get; private set; }

        /// <summary>Human-readable explanation of why this classification was chosen.</summary>
        public string Reason { get; private set; } = string.Empty;

        /// <summary>When the decision was made.</summary>
        public DateTimeOffset DecidedAt { get; private set; }

        /// <summary>Whether a rerun was attempted before the decision was reached.</summary>
        public bool RerunAttempted { get; private set; }

        /// <summary>Whether the rerun succeeded (null if no rerun was attempted).</summary>
        public bool? RerunSucceeded { get; private set; }

        // Private constructor for ORM / rehydration
        private Decision() { }

        public static Decision Create(
            Guid incidentId,
            IncidentClassification classification,
            string reason,
            bool rerunAttempted,
            bool? rerunSucceeded)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(reason);

            return new Decision
            {
                Id = Guid.NewGuid(),
                IncidentId = incidentId,
                Classification = classification,
                Reason = reason,
                RerunAttempted = rerunAttempted,
                RerunSucceeded = rerunSucceeded,
                DecidedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
