using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetPagePreviewByIdQuery : IRequest<BaseMediatrResponse<GetPagePreviewByIdQueryResponse>>
{
    public readonly Guid PageId;
    public readonly Guid SectionId;
    public readonly Guid FormVersionId;

    public GetPagePreviewByIdQuery(Guid pageId, Guid sectionId, Guid formVersionId)
    {
        PageId = pageId;
        SectionId = sectionId;
        FormVersionId = formVersionId;
    }
}
