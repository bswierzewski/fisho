using Fishio.Infrastructure.Services;      // Dla CurrentUserService
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect; // Dla OpenIdConnectConfiguration
using Microsoft.IdentityModel.Protocols;             // Dla ConfigurationManager
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Fishio.API.Infrastructure; // Dla OpenApiSecurityScheme etc.

namespace Web; // Zakładam, że namespace to 'Web' zgodnie z Twoim plikiem

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
                options.Authority = clerkAuthority;
                options.Audience = clerkAudience;
                options.RequireHttpsMetadata = builder.Environment.IsProduction();

                // Automatyczne pobieranie konfiguracji OpenID Connect (w tym JWKS URI)
                // To jest preferowane podejście, zamiast ręcznego ustawiania JwksUri.
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{clerkAuthority.TrimEnd('/')}/.well-known/openid-configuration", // Upewnij się, że URL jest poprawny
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever { RequireHttps = options.RequireHttpsMetadata }); // Użyj RequireHttps z opcji

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = clerkAuthority,

                    ValidateAudience = true,
                    ValidAudience = clerkAudience,

                    ValidateIssuerSigningKey = true, // Kluczowe, JWKS URI zostanie użyte do pobrania kluczy
                    // Klucze publiczne zostaną pobrane z JWKS URI dostarczonego przez Authority.
                    // Nie ma potrzeby ręcznego ustawiania IssuerSigningKey, jeśli JWKS jest dostępne.

                    NameClaimType = "sub", // Standardowy claim dla ID użytkownika w OIDC (Clerk go używa)
                                           // Możesz też użyć ClaimTypes.NameIdentifier, jeśli tak jest skonfigurowany Clerk.
                    RoleClaimType = "permissions", // Jeśli Clerk używa claimu "permissions" dla ról/uprawnień.
                                                   // Dostosuj, jeśli nazwa claimu jest inna.

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30) // Niewielka tolerancja dla różnic zegarów, standardowo 5 minut.
                                                         // TimeSpan.Zero może być zbyt restrykcyjne.
                };

                // Opcjonalnie: Obsługa zdarzeń walidacji tokenu dla debugowania
                // options.Events = new JwtBearerEvents
                // {
                //     OnAuthenticationFailed = context =>
                //     {
                //         // Logowanie błędów
                //         Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                //         return Task.CompletedTask;
                //     },
                //     OnTokenValidated = context =>
                //     {
                //         // Token pomyślnie zwalidowany
                //         Console.WriteLine("Token validated for user: " + context.Principal?.Identity?.Name);
                //         return Task.CompletedTask;
                //     }
                // };
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
                // Możesz dodać bardziej restrykcyjne nazwane polityki CORS dla produkcji
                // options.AddPolicy("AllowFishioFrontend", policy =>
                // {
                //     policy.WithOrigins("https://twoj-frontend.com")
                //           .AllowAnyMethod()
                //           .AllowAnyHeader();
                // });
            });
        }
    }
}
