using MediatR;

namespace SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;

public class GetFormVersionByIdQuery : IRequest<GetFormVersionByIdQueryResponse>
{
    public readonly Guid FormVersionId;

    public GetFormVersionByIdQuery(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}