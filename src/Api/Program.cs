using Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Enables IExceptionHandler + RFC 7807 ProblemDetails responses
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<Api.Middleware.GlobalExceptionHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

// Exposes Program to the integration test WebApplicationFactory
public partial class Program;
