using MediatR;
using SFA.DAS.Aodp.Application;

namespace SFA.DAS.Aodp.Application.Queries.Application.Section;

public class GetApplicationSectionByIdQuery : IRequest<BaseMediatrResponse<GetApplicationSectionByIdQueryResponse>>
{
    public GetApplicationSectionByIdQuery(Guid sectionId, Guid formVersionId)
    {
        SectionId = sectionId;
        FormVersionId = formVersionId;
    }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}
