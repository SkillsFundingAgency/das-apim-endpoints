using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

public class GetOrganisationRequest : IGetApiRequest
{
    public Guid OrganisationId { get; set; }

    public GetOrganisationRequest(Guid organisationId)
    {
        OrganisationId = organisationId;
    }

    public string GetUrl => $"/api/v1/organisations/organisation/{OrganisationId}";
}
