using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Forms;

public class GetFormVersionByIdQuery : IRequest<BaseMediatrResponse<GetFormVersionByIdQueryResponse>>
{
    public readonly Guid FormVersionId;

    public GetFormVersionByIdQuery(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}