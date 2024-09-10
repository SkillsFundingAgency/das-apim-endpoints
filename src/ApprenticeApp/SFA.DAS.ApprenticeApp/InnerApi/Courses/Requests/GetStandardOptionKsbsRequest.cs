using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.Courses.Requests
{
    public class GetStandardOptionKsbsRequest : IGetApiRequest
    {
        public string StandardUid { get; }
        public string Option { get; }
        public string GetUrl => $"api/courses/Standards/{StandardUid}/options/{Option}/ksbs";

        public GetStandardOptionKsbsRequest(string id, string option)
        {
            StandardUid = id;
            Option = option;
        }
    }
}