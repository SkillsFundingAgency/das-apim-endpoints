using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Assessor
{
    public class GetEndpointAssessmentsRequest : IGetApiRequest
    {
        public GetEndpointAssessmentsRequest(int ukprn)
        {
            Ukprn = ukprn;
        }
        public int Ukprn { get; }
        public string GetUrl => $"api/ao/assessments?ukprn={Ukprn}";
    }
}
