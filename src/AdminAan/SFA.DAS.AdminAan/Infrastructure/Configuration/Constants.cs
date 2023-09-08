using System.ComponentModel;

namespace SFA.DAS.AdminAan.Infrastructure.Configuration;
public static class Constants
{
    public static class ApiHeaders
    {
        public const string RequestedByMemberIdHeader = "X-RequestedByMemberId";
    }

    public static class AdminSettings
    {
        public const int PageSize = 10;
    }

    public enum Status
    {
        [Description("Live")]
        Live
    }
}
