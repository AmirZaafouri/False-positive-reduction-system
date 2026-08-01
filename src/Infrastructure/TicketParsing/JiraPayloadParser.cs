using System.Text.Json;
using Application.Common;
using Application.Interfaces;
namespace Infrastructure.TicketParsing;

public class JiraPayloadParser : ITicketPayloadParser
{


    public string ProviderName => "Jira";

    public IncidentIntake Parse(string rawPayload)
    {
        using var doc = JsonDocument.Parse(rawPayload);
        var root = doc.RootElement;

        return new IncidentIntake
        {
            TicketId = GetString(root, "ticketId") ?? "UNKNOWN",
            Summary = GetString(root, "summary") ?? string.Empty,
            Status = GetString(root, "status"),
            IssueType = GetString(root, "issueType"),
            Environment = GetString(root, "environment"),
            CreatedAt = TryGetDate(root, "createdAt"),
            SourceProvider = ProviderName,
            RawPayload = rawPayload
        };
    }

    private static string? GetString(JsonElement root, string propertyName)
    {
        return root.TryGetProperty(propertyName, out var value) && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;
    }

    private static DateTimeOffset? TryGetDate(JsonElement root, string propertyName)
    {
        var raw = GetString(root, propertyName);
        return DateTimeOffset.TryParse(raw, out var parsed) ? parsed : null;
    }
}
