using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

public class GetApplicationsByOrganisationIdApiRequest : IGetApiRequest
{
    public Guid OrganisationId { get; set; }

    public string GetUrl => $"/api/applications/organisations/{OrganisationId}";
}
