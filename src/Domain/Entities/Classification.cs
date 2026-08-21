using Domain.Enums;

namespace Domain.Entities
{
    /// <summary>
    /// Captures a single classification label and its confidence score.
    /// Used by the decision engine to record one candidate classification
    /// before a final Decision is committed.
    /// </summary>
    public class Classification
    {
        public Guid Id { get; private set; }

        /// <summary>The incident this classification applies to.</summary>
        public Guid IncidentId { get; private set; }

        /// <summary>The assigned classification label.</summary>
        public IncidentClassification Label { get; private set; }

        /// <summary>
        /// Confidence score between 0.0 and 1.0.
        /// 1.0 = rule-based certainty; values below 1.0 are AI-assigned.
        /// </summary>
        public double Confidence { get; private set; }

        /// <summary>Source of this classification (e.g. "RuleBased", "LLM").</summary>
        public string Source { get; private set; } = string.Empty;

        /// <summary>When this classification was produced.</summary>
        public DateTimeOffset ClassifiedAt { get; private set; }

        // Private constructor for ORM / rehydration
        private Classification() { }

        public static Classification Create(
            Guid incidentId,
            IncidentClassification label,
            double confidence,
            string source)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(source);

            if (confidence is < 0.0 or > 1.0)
                throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence must be between 0.0 and 1.0.");

            return new Classification
            {
                Id = Guid.NewGuid(),
                IncidentId = incidentId,
                Label = label,
                Confidence = confidence,
                Source = source,
                ClassifiedAt = DateTimeOffset.UtcNow
            };
        }
    }
}
