using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.TrackProgressInternal.Application.Services;

public static class HostingEnvironmentExtensions
{
    public static bool IsLocalOrDev(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsLocal() || hostEnvironment.IsDev() || hostEnvironment.IsLocalAcceptanceTests();
    }

    public static bool IsLocalAcceptanceTests(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("LOCAL_ACCEPTANCE_TESTS");
    }

    public static bool IsDev(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("DEV");
    }

    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment("LOCAL");
    }
}