using System.Text.Json.Serialization;
using Fishio.API.Infrastructure;
using Fishio.Infrastructure.Filter; // Dla OpenApiSecurityScheme etc.
using Fishio.Infrastructure.Services;      // Dla CurrentUserService
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Fishio.Application.Common.Options;
using Fishio.Domain.Constants;

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

            // Add JWT Bearer security definition
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
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
        builder.Services.Configure<ClerkOptions>(builder.Configuration.GetSection(ClerkOptions.SectionName));
        var clerkOptions = builder.Configuration.GetSection(ClerkOptions.SectionName).Get<ClerkOptions>()
            ?? throw new InvalidOperationException($"Configuration section '{ClerkOptions.SectionName}' not found or empty.");

        if (string.IsNullOrWhiteSpace(clerkOptions.Authority))
        {
            throw new InvalidOperationException($"'{nameof(ClerkOptions.Authority)}' must be configured in section '{ClerkOptions.SectionName}'.");
        }
        if (string.IsNullOrWhiteSpace(clerkOptions.Audience))
        {
            throw new InvalidOperationException($"'{nameof(ClerkOptions.Audience)}' must be configured in section '{ClerkOptions.SectionName}'.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = clerkOptions.Authority;     // Z pola "iss"
                options.Audience = clerkOptions.Audience;      // Z pola "azp" (gdy "aud" brak)

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = clerkOptions.Authority, // Powtórzenie dla jawnej walidacji

                    ValidateAudience = true,
                    ValidAudience = clerkOptions.Audience,       // Powtórzenie dla jawnej walidacji

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
            options.AddPolicy(PolicyNames.OrganizerPolicy, policy =>
                policy.RequireAuthenticatedUser()); // Możesz dodać .RequireClaim("permissions", "manage:competitions")
            options.AddPolicy(PolicyNames.JudgePolicy, policy =>
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
