using MediatR;

public class GetRelatedQualificationForApplicationQuery : IRequest<BaseMediatrResponse<GetRelatedQualificationForApplicationQueryResponse>>
{
    public GetRelatedQualificationForApplicationQuery(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
    public Guid ApplicationId { get; set; }
}
