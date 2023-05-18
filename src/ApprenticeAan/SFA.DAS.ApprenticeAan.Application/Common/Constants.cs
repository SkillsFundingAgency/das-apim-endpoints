namespace SFA.DAS.ApprenticeAan.Application.Common;

public static class Constants
{
    public static class ApiHeaders
    {
        public const string RequestedByMemberId = "X-RequestedByMemberId";
    }
    public static class AanHubApiUrls
    {
        public const string GetRegionsUrl = $"/regions";
        public const string GetProfilesUrl = $"/profiles/";
    }
}