using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EducationalOrganisations
{
    public class PostEducationOrganisationsDataLoadRequest : IPostApiRequest
    {
        public string PostUrl => "ops/dataload";
        public object Data { get; set; }
    }
}