using Application.Common;

namespace Application.Interfaces
{
    public interface ITicketPayloadParser
    {
        string ProviderName { get; }
        IncidentIntake Parse(string rawPayload);
    }
}
