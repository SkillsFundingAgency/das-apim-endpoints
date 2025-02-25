using MediatR;

public class GetApplicationFormPreviewByIdQuery : IRequest<BaseMediatrResponse<GetApplicationFormPreviewByIdQueryResponse>>
{
    public Guid ApplicationId { get; set; }

    public GetApplicationFormPreviewByIdQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}
