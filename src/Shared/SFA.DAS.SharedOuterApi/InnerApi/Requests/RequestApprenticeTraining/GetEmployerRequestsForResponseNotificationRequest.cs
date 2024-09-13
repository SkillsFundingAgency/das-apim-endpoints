using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class GetEmployerRequestsForResponseNotificationRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employerrequest/requests-for-response-notification";
    }
}
