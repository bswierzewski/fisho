using System.Text.Json.Serialization;
using Fishio.API.Infrastructure;
using Fishio.Infrastructure.Filter; // Dla OpenApiSecurityScheme etc.
using Fishio.Infrastructure.Services;      // Dla CurrentUserService
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Fishio.API;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services; // Dla czytelności

        // --- Konfiguracja Kontrolerów (jeśli używasz, choć projekt jest Minimal API) ---
        // Jeśli projekt jest czysto Minimal API, ta linia może nie być potrzebna.
        // Jeśli masz jakieś kontrolery (np. dla widoków błędów), zostaw.
        //services.AddControllers();

        // --- Konfiguracja Swagger/OpenAPI ---
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Fishio API", Version = "v1" });
            options.SchemaFilter<EnumSchemaFilter>();
        });

        services.ConfigureHttpJsonOptions(services =>
        {
            // Ustawienia dla JSON, jeśli używasz System.Text.Json
            services.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // --- Konfiguracja Zachowania API ---
        // Wyłącza automatyczną walidację ModelState przez filtr [ApiController],
        // co daje większą kontrolę, jeśli używasz np. FluentValidation z MediatR.
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // --- Konfiguracja Obsługi Wyjątków ---
        services.AddExceptionHandler<CustomExceptionHandler>(); // Odkomentuj, jeśli masz zaimplementowany

        // --- Konfiguracja Dostępności HttpContext ---
        services.AddHttpContextAccessor();

        // Rejestracja ICurrentUserService
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // --- Konfiguracja Uwierzytelniania JWT (dla Clerk) ---
        // Pobieranie konfiguracji Clerk z appsettings.json lub zmiennych środowiskowych
        // Zmieniono nazwy kluczy konfiguracyjnych na bardziej spójne z moimi sugestiami
        var clerkAuthority = builder.Configuration["Clerk:Authority"]
            ?? throw new ArgumentNullException("Clerk:Authority", "Clerk Authority URL must be configured.");
        var clerkAudience = builder.Configuration["Clerk:Audience"]
            ?? throw new ArgumentNullException("Clerk:Audience", "Clerk Audience must be configured.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = clerkAuthority;     // Z pola "iss"
                options.Audience = clerkAudience;      // Z pola "azp" (gdy "aud" brak)

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = clerkAuthority, // Powtórzenie dla jawnej walidacji

                    ValidateAudience = true,
                    ValidAudience = clerkAudience,       // Powtórzenie dla jawnej walidacji

                    ValidateIssuerSigningKey = true,
                    // Klucze zostaną pobrane z JWKS URI dostarczonego przez Authority

                    NameClaimType = "sub",
                    RoleClaimType = "permissions", // Dostosuj, jeśli Clerk używa innego claimu dla ról/uprawnień

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30)
                };
            });

        // --- Konfiguracja Autoryzacji (Polityki) ---
        services.AddAuthorization(options =>
        {
            // Przykładowe polityki - dostosuj do swoich potrzeb
            options.AddPolicy("OrganizerPolicy", policy =>
                policy.RequireAuthenticatedUser()); // Możesz dodać .RequireClaim("permissions", "manage:competitions")
            options.AddPolicy("JudgePolicy", policy =>
                policy.RequireAuthenticatedUser());
            // ... inne polityki ...
        });


        // --- Konfiguracja CORS (tylko w trybie deweloperskim w tym przykładzie) ---
        if (builder.Environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder => // Zmieniono nazwę zmiennej dla jasności
                {
                    policyBuilder.AllowAnyOrigin()
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                });
            });
        }
    }
}
