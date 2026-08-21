namespace Application.Interfaces
{
    public interface IIncidentIntakeService
    {
        Task ProcessAsync(string rawPayload);
    }
}
