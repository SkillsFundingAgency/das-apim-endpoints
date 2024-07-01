using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetStandardDetailsByIdRequest : IGetApiRequest
    {
        public string Id { get; }
        public string GetUrl => $"api/courses/standards/{Id}";

        public GetStandardDetailsByIdRequest(string id)
        {
            Id = id;
        }
    }
}