using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations
{
    public class PostEducationOrganisationsDataLoadRequest : IPostApiRequest
    {
        public string PostUrl => "ops/dataload";
        public object Data { get; set; }
    }
}