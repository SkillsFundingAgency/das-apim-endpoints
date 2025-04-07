using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationsByOrganisationIdQuery : IRequest<BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse>>
{
    public GetApplicationsByOrganisationIdQuery(Guid organisationId)
    {
        OrganisationId = organisationId;
    }
    public Guid OrganisationId { get; set; }


}
