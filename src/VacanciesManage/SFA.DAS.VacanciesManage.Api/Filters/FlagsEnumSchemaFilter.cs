using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SFA.DAS.VacanciesManage.Api.Filters;

[ExcludeFromCodeCoverage]
public class FlagsEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type?.IsEnum != true || !context.Type.IsDefined(typeof(FlagsAttribute), false))
            return;

        schema.Type = "integer";
        schema.Format = "int64";
        schema.Enum = Enum.GetValues(context.Type)
            .Cast<object>()
            .Select(v => (IOpenApiAny)new OpenApiLong((long)Convert.ChangeType(v, typeof(long))))
            .ToList();
        schema.Extensions["x-enumFlags"] = new OpenApiBoolean(true);
        var names = new OpenApiArray();
        foreach (var name in Enum.GetNames(context.Type))
            names.Add(new OpenApiString(name));
        schema.Extensions["x-enumNames"] = names;
        schema.Description = "Flags enum — combine values with bitwise OR, or pass comma-separated names (e.g. ShortDescription,Title).";
    }
}