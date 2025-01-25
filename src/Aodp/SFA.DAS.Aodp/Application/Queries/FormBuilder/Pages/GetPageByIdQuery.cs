using MediatR;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Pages;

public class GetPageByIdQuery : IRequest<GetPageByIdQueryResponse>
{
    public Guid PageId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}