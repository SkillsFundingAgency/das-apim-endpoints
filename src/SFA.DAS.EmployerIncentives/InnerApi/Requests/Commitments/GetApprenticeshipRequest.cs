using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments
{
    public class GetApprenticeshipRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetApprenticeshipRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId;
        }

        public string BaseUrl { get; set; }

        public string GetUrl =>
            $"{BaseUrl}api/apprenticeships/{_apprenticeshipId}";
    }
}