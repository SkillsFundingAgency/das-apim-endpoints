using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses
{
    public class GetStandardRequest : IGetApiRequest
    {
        public GetStandardRequest(int id)
        {
            StandardId = id;
        }

        public int StandardId { get; }
        public string GetUrl => $"api/courses/standards/{StandardId}";
    }
}