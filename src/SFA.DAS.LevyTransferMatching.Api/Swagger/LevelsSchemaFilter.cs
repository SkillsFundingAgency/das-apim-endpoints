using System.Collections.Generic;
using MediatR;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SFA.DAS.LevyTransferMatching.Api.Swagger
{
    public class LevelsSchemaFilter : ISchemaFilter
    {
        private readonly IMediator _mediator;

        public LevelsSchemaFilter(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var levels = _mediator.Send(new GetLevelsQuery()).GetAwaiter().GetResult();

            var x = schema.Properties;

            if (x.TryGetValue("levels", out var schemaValue))
            {
                if (schemaValue.Type == "array")
                {
                    //schemaValue.Reference = new OpenApiReference
                    //    {ExternalResource = "https://localhost:5221/tags/levels2", Type = ReferenceType.Link};

                    if (schemaValue.Items.Type == "string")
                    {
                        schemaValue.Nullable = false;
                        schemaValue.Type = "string";
                        schemaValue.Enum = new List<IOpenApiAny>();

                        foreach (var tag in levels.Tags)
                        {
                            schemaValue.Enum.Add(new OpenApiString(tag.TagId));
                        }
                    }
                }

            }



        }
    }
}
