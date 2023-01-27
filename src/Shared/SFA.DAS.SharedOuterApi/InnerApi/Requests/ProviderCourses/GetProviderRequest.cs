using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCourses
{
    public class GetProviderRequest : IGetApiRequest
    {
        private readonly int _id;

        public GetProviderRequest(int id)
        {
            _id = id;
        }

        public string GetUrl => $"api/providers/{_id}";
    }
}
