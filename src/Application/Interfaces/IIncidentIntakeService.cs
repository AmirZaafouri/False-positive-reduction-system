using Application.Common;

namespace Application.Interfaces
{
    public interface IIncidentIntakeService
    {
        string ProviderName { get; }
        Task ProcessAsync(string rawPayload);
    }
}
