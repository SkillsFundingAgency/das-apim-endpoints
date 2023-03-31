using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;

        public GetAllStandardsRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/trainingprogramme/standards";
    }
}
