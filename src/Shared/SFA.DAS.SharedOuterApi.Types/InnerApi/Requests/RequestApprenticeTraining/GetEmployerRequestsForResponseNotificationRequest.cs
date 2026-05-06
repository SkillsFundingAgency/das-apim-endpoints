using System.Diagnostics.CodeAnalysis;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining
{
    [ExcludeFromCodeCoverage]
    public class GetEmployerRequestsForResponseNotificationRequest : IGetApiRequest
    {
        public string GetUrl => $"api/employer-requests/response-notifications";
    }
}
