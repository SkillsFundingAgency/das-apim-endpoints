using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses
{
    public class GetAllStandardsRequest : IGetApiRequest
    {
        public string GetUrl => $"api/trainingprogramme/standards";
    }
}
