using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RequestApprenticeTraining;

[ExcludeFromCodeCoverage]
public class GetEmployerRequestsForResponseNotificationRequest : IGetApiRequest
{
    public string GetUrl => $"api/employer-requests/response-notifications";
}