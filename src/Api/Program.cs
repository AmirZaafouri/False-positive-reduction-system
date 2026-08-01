


using Application.Interfaces;

using Infrastructure.TicketParsing;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITicketPayloadParser, JiraPayloadParser>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/webhooks/jira", async (
    HttpRequest request,
    ITicketPayloadParser parser,
    ILogger<Program> logger) =>
{
    using var reader = new StreamReader(request.Body);
    var rawPayload = await reader.ReadToEndAsync();

    var incident = parser.Parse(rawPayload);

    logger.LogInformation(
        "Parsed incident intake: TicketId={TicketId}, Summary={Summary}, Status={Status}, IssueType={IssueType}, Provider={Provider}",
        incident.TicketId, incident.Summary, incident.Status, incident.IssueType, incident.SourceProvider);

    return Results.Accepted();
});





app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
