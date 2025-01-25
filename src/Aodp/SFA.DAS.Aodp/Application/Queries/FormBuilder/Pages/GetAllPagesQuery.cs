using MediatR;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Pages;

public class GetAllPagesQuery : IRequest<GetAllPagesQueryResponse>
{
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}