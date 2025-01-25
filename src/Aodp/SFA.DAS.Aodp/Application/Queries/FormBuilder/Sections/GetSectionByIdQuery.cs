using MediatR;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Sections;

public class GetSectionByIdQuery : IRequest<GetSectionByIdQueryResponse>
{
    public readonly Guid SectionId;
    public readonly Guid FormVersionId;

    public GetSectionByIdQuery(Guid sectionId, Guid formVersionId)
    {
        SectionId = sectionId;
        FormVersionId = formVersionId;
    }
}