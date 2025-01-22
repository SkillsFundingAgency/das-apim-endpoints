using MediatR;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetPageByIdQuery : IRequest<GetPageByIdQueryResponse>
{
    public readonly Guid PageId;
    public readonly Guid SectionId;

    public GetPageByIdQuery(Guid pageId, Guid sectionId)
    {
        PageId = pageId;
        SectionId = sectionId;
    }
}