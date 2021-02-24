using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardDetailsByStandardUIdRequest : IGetApiRequest
    {
        public string StandardUId { get; }
        public string GetUrl => $"api/courses/standards/{StandardUId}";

        public GetStandardDetailsByStandardUIdRequest(string standardUId)
        {
            StandardUId = standardUId;
        }
    }
}