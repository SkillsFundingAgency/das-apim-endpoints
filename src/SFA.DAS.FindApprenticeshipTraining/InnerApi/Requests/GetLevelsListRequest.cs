using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLevelsListRequest : IGetApiRequest
    {
        public string GetUrl => "api/courses/levels";
    }
}