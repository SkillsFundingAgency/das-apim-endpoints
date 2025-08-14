﻿using Microsoft.Extensions.Options;
using SFA.DAS.SharedOuterApi.Configuration;

namespace SFA.DAS.Admin.Roatp.Api.AppStart;

public static class AddConfigurationOptionsExtension
{
    public static IServiceCollection AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<RoatpConfiguration>(configuration.GetSection("RoatpApiConfiguration"));
        services.AddSingleton(cfg => cfg.GetService<IOptions<RoatpConfiguration>>()!.Value);
        return services;
    }
}
