using MediatR;

public class GetApplicationsByOrganisationIdQuery : IRequest<BaseMediatrResponse<GetApplicationsByOrganisationIdQueryResponse>>
{
    public GetApplicationsByOrganisationIdQuery(Guid organisationId)
    {
        OrganisationId = organisationId;
    }
    public Guid OrganisationId { get; set; }


}
