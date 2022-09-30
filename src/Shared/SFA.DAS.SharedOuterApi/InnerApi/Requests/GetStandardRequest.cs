using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardRequest : IGetApiRequest
    {
        public GetStandardRequest(string standardUId)
        {
            StandardId = standardUId;
        }

        public string StandardId { get; }
        public string GetUrl => $"api/courses/standards/{StandardId}";
    }
}