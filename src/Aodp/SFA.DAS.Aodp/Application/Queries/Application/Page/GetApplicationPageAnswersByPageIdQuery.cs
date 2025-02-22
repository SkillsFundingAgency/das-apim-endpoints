using MediatR;
using SFA.DAS.Aodp.Application;

public class GetApplicationPageAnswersByPageIdQuery : IRequest<BaseMediatrResponse<GetApplicationPageAnswersByPageIdQueryResponse>>
{
    public GetApplicationPageAnswersByPageIdQuery(Guid applicationId, Guid pageId, Guid sectionId, Guid formVersionId)
    {
        ApplicationId = applicationId;
        PageId = pageId;
        SectionId = sectionId;
        FormVersionId = formVersionId;
    }

    public Guid PageId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}
