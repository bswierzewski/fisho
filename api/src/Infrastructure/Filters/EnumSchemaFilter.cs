// Umieść to w odpowiednim miejscu, np. w Fishio.API/Infrastructure
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fishio.Infrastructure.Filter;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear(); // Usuń wartości numeryczne
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(name => schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString(name)));
            schema.Type = "string"; // Zmień typ schematu na string
            schema.Format = null;   // Usuń format, jeśli był (np. int32)
        }
    }
}
