using Microsoft.AspNetCore.Mvc.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.RoatpOversight.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class AddControllersExtension
{
    public static IServiceCollection AddControllers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(o =>
        {
            if (!configuration.IsLocalOrDev()) o.Filters.Add(new AuthorizeFilter("default"));
        });
        return services;
    }
}
