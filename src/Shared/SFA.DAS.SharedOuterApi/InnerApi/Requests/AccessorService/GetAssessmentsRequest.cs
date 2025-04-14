using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.AccessorService;

public sealed class GetAssessmentsRequest : IGetApiRequest
{
    public long Ukprn { get; }
    public string IFateReferenceNumber { get; }

    public GetAssessmentsRequest(long ukprn, string ifateReferenceNumber)
    {
        Ukprn = ukprn;
        IFateReferenceNumber = ifateReferenceNumber;
    }

    public string GetUrl => $"api/ao/assessments?standard={IFateReferenceNumber}&ukprn={Ukprn}";
}
