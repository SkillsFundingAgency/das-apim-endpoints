namespace SFA.DAS.ApprenticeAan.Application.Common
{
    public static class Constants
    {
        public static class AanHubApiUrls
        {
            private const string ApiPrefix = "/api";
            public const string GetRegionsUrl = $"{ApiPrefix}/regions";
            public const string GetProfilesUrl = $"{ApiPrefix}/profiles?userType=";
        }
    }
}