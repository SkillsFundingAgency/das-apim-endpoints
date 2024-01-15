using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests
{
    public class GetApplicationApiRequest : IGetApiRequest
    {
        private readonly string _vacancyReference;

        public GetApplicationApiRequest(string vacancyReference)
        {
            _vacancyReference = vacancyReference;
        }

        public string GetUrl => $"/api/candidates/XYZ/applications/{_vacancyReference}";
    }
}
