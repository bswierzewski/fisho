using Application;
using Fishio.API.Endpoints;
using Fishio.API.Middlewares;
using Infrastructure.Persistence;
using Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

await app.InitialiseDatabaseAsync();
if (app.Environment.IsDevelopment()) { await app.SeedDatabaseAsync(); }

// Swagger tylko w Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fishio API V1"));
}
// Middleware do obsługi błędów
app.UseExceptionHandler(options => { });

app.UseCors();

app.UseAuthentication();    // 1. Uwierzytelnianie (np. Clerk JWT validation)
app.UseMiddleware<UserProvisioningMiddleware>();  // 2. Nasze middleware do mapowania/provisioningu użytkownika domenowego
app.UseAuthorization();     // 3. Autoryzacja

// Mapowanie endpointów - jeśli nie jest częścią AddWebServices
// app.MapControllers(); // Jeśli używasz kontrolerów

// Mapowanie endpointów
app.MapAboutEndpoints();
app.MapCompetitionEndpoints();
app.MapDashboardEndpoints();
app.MapFisheriesEndpoints();
app.MapLogbookEndpoints();
app.MapLookupDataEndpoints();
app.MapPublicResultsEndpoints();
app.MapUserEndpoints();

// 5. Uruchomienie Aplikacji
app.Run();
