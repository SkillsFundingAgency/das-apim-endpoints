using MediatR;
using SFA.DAS.Aodp.Application;


public class GetApplicationMetadataByIdQuery : IRequest<BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>>
{
    public GetApplicationMetadataByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
    public Guid ApplicationId { get; set; }
}
