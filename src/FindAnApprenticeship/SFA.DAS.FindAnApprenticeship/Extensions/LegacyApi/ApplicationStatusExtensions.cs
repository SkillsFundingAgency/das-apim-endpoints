using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Extensions.LegacyApi
{
    public static class ApplicationStatusExtensions
    {
        public static ApplicationStatus ToFaaApplicationStatus(InnerApi.LegacyApi.Responses.Enums.ApplicationStatus source)
        {
            switch (source)
            {
                case InnerApi.LegacyApi.Responses.Enums.ApplicationStatus.Draft:
                    return ApplicationStatus.Draft;
                case InnerApi.LegacyApi.Responses.Enums.ApplicationStatus.Submitted:
                    return ApplicationStatus.Submitted;
                case InnerApi.LegacyApi.Responses.Enums.ApplicationStatus.Successful:
                    return ApplicationStatus.Successful;
                case InnerApi.LegacyApi.Responses.Enums.ApplicationStatus.Unsuccessful:
                    return ApplicationStatus.UnSuccessful;
                default:
                    return ApplicationStatus.Withdrawn; //todo: finish this mapping
            }
        }
    }
}
