using Microsoft.OpenApi.Models;
using SFA.DAS.SharedOuterApi.AppStart;

namespace SFA.DAS.AdminRoatp.Api.AppStart;

public static class AddSwaggerExtension
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "AdminRoatpOuterApi",
                    Version = "v1"
                });

            if (!configuration.IsLocalOrDev())
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
            }
        });
        return services;
    }
}
