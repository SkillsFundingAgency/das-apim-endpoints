using MediatR;

namespace SFA.DAS.AODP.Application.FormBuilder.Pages.Queries;

public class GetAllPagesQuery : IRequest<GetAllPagesQueryResponse>
{
    public readonly Guid SectionId;

    public GetAllPagesQuery(Guid sectionId)
    {
        SectionId = sectionId;
    }
}