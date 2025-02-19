using MediatR;


public class GetApplicationMetadataByIdQuery : IRequest<BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>>
{
    public GetApplicationMetadataByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
    public Guid ApplicationId { get; set; }
}
