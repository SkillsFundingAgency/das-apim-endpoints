using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ReferenceDataJobs.InnerApi.Requests;

public class PostPublicSectorOrganisationsDataLoadRequest : IPostApiRequest
{
    public string PostUrl => "dataload/start";
    public object Data { get; set; }
}