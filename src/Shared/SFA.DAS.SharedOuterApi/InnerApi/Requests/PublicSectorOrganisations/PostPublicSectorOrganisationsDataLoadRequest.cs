using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations
{
    public class PostPublicSectorOrganisationsDataLoadRequest : IPostApiRequest
    {
        public string PostUrl => "dataload/start";
        public object Data { get; set; }
    }
}