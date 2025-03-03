using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

public class GetApprenticeshipsRequest : IGetApiRequest
{
    public long Ukprn { get; set; }
    public string GetUrl => $"{Ukprn}";
}