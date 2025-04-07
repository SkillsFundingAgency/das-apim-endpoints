using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Application.Application;

public class GetApplicationFormPreviewByIdQuery : IRequest<BaseMediatrResponse<GetApplicationFormPreviewByIdQueryResponse>>
{
    public Guid ApplicationId { get; set; }

    public GetApplicationFormPreviewByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
