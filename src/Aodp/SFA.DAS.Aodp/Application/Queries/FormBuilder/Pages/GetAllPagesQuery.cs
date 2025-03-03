using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Pages;

public class GetAllPagesQuery : IRequest<BaseMediatrResponse<GetAllPagesQueryResponse>>
{
    public Guid SectionId { get; set; }
    public Guid FormVersionId { get; set; }

}