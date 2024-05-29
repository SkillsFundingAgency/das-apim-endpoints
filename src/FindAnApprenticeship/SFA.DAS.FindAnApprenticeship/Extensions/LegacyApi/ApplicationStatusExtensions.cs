using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.Extensions.LegacyApi
{
    public static class ApplicationStatusExtensions
    {
        public static ApplicationStatus ToFaaApplicationStatus(this InnerApi.LegacyApi.Responses.Enums.ApplicationStatus source)
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
                    throw new InvalidOperationException($"Unable to convert status {source} to FAA Application Status");
            }
        }
    }
}
