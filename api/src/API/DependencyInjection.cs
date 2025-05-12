using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Web;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //builder.Services.AddFluentValidationRulesToSwagger();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            // Konfiguracja Swaggera do obsługi JWT
            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        builder.Services.AddHttpContextAccessor();

        var clerkIssuer = builder.Configuration["Clerk:Issuer"] ?? throw new ArgumentNullException("Clerk:Issuer");
        var clerkAudience = builder.Configuration["Clerk:Audience"] ?? throw new ArgumentNullException("Clerk:Audience");
        var clerkJwksUri = builder.Configuration["Clerk:JwksUri"];

        // Authentication and Authorization
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = clerkIssuer; // Issuer z Twojego dashboardu Clerk
                options.Audience = clerkAudience;  // Często issuer i audience są takie same dla Clerk
                options.RequireHttpsMetadata = builder.Environment.IsProduction(); // Wymagaj HTTPS na produkcji

                // Konfiguracja pobierania kluczy publicznych JWKS
                options.ConfigurationManager = new Microsoft.IdentityModel.Protocols.ConfigurationManager<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration>(
                    $"{clerkIssuer}/.well-known/openid-configuration",
                    new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfigurationRetriever(),
                    new Microsoft.IdentityModel.Protocols.HttpDocumentRetriever());

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = clerkIssuer,
                    ValidateAudience = true, // Zazwyczaj true, jeśli Audience jest ustawione
                    ValidAudience = clerkAudience, // Lub specyficzna publiczna publiczna wartość Audience, jeśli ją masz
                    ValidateIssuerSigningKey = true, // Kluczowe dla weryfikacji podpisu
                                                     // NameClaimType i RoleClaimType mogą być potrzebne, jeśli używasz ról z JWT
                    NameClaimType = ClaimTypes.NameIdentifier, // Mapuje 'sub' z JWT na User.Identity.Name
                                                               // RoleClaimType = "rola_z_jwt", // Jeśli masz role w tokenie
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Brak tolerancji dla wygaśnięcia tokenu
                };
            });
        builder.Services.AddAuthorization();

        // Add CORS services only in development mode
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
        }
    }
}
