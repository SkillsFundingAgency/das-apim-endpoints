using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.AccessorService;

public sealed class GetAssessmentsRequest(long ukprn, string ifateReferenceNumber) : IGetApiRequest
{
    public long Ukprn { get; } = ukprn;
    public string IFateReferenceNumber { get; } = ifateReferenceNumber;

    public string GetUrl => $"api/ao/assessments?standard={IFateReferenceNumber}&ukprn={Ukprn}";
}
