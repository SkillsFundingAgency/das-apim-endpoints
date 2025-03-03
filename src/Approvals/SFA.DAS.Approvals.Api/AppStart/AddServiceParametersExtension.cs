﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Approvals.Application;

namespace SFA.DAS.Approvals.Api.AppStart
{
    public static class AddServiceParametersExtension
    {
        public static IServiceCollection AddServiceParameters(this IServiceCollection services)
        {
            services.AddScoped(provider => {
                var httpContext = provider.GetRequiredService<IHttpContextAccessor>().HttpContext;

                if (httpContext.Request.Path.Value.StartsWith("/employer"))
                {
                    if (long.TryParse((string)httpContext.Request.RouteValues["accountId"], out var accountId))
                    {
                        return new ServiceParameters(Application.Shared.Enums.Party.Employer, accountId);
                    }
                }

                if (httpContext.Request.Path.Value.StartsWith("/provider"))
                {
                    if (long.TryParse((string)httpContext.Request.RouteValues["providerId"], out var providerId))
                    {
                        return new ServiceParameters(Application.Shared.Enums.Party.Provider, providerId);
                    }
                }

                if (httpContext.Request.Path.Value.StartsWith("/transfer-sender"))
                {
                    if (long.TryParse((string)httpContext.Request.RouteValues["transferSenderId"], out var transferSenderId))
                    {
                        return new ServiceParameters(Application.Shared.Enums.Party.TransferSender, transferSenderId);
                    }
                }

                return new ServiceParameters(Application.Shared.Enums.Party.None, 0);

            });

            return services;
        }
    }
}
