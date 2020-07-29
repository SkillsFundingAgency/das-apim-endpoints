using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetLevelsListRequest : IGetApiRequest
    {
        public string BaseUrl { get; set; }
        public string GetUrl => $"{BaseUrl}api/courses/levels";
    }
}