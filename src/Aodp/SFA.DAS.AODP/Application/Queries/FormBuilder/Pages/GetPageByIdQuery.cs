using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetPageByIdQuery : IRequest<BaseMediatrResponse<GetPageByIdQueryResponse>>
{
    public Guid PageId { get; set; }
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}