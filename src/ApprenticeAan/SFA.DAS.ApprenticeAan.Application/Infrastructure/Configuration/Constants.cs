using System.ComponentModel;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;
public static class Constants
{
    public static class ApiHeaders
    {
        public const string RequestedByMemberIdHeader = "X-RequestedByMemberId";
    }

    public enum Status
    {
        [Description("Live")]
        Live
    }
}