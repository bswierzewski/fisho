using API.Middlewares;
using Application;
using Infrastructure.Persistence;
using Web;
using Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

await app.InitialiseDatabaseAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(); // Applies the default policy

    app.UseSwagger();
    app.UseSwaggerUI();

    await app.SeedDatabaseAsync();
}

app.UseRouting(); // Ważne dla działania middleware

app.UseAuthentication(); // Najpierw uwierzytelnianie
app.UseAuthorization();  // Potem autoryzacja

app.UseExceptionHandler(options => { });

app.UseMiddleware<UserSyncMiddleware>();

app.MapEndpoints();

app.Run();
