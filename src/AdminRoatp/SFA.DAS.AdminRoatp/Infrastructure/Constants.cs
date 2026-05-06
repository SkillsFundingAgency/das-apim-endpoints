using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AdminRoatp.Infrastructure;

[ExcludeFromCodeCoverage]
public static class Constants
{
    public const string RequestingUserIdHeader = "X-RequestingUserId";
    public const string RequestingUserNameHeader = "X-RequestingUserName";
}
