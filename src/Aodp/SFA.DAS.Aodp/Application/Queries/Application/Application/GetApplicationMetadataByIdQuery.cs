using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationMetadataByIdQuery : IRequest<BaseMediatrResponse<GetApplicationMetadataByIdQueryResponse>>
{
    public GetApplicationMetadataByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
    public Guid ApplicationId { get; set; }
}
