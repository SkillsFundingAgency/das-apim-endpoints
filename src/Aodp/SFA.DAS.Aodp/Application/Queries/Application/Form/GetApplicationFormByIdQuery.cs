using MediatR;
using SFA.DAS.Aodp.Application;

namespace SFA.DAS.Aodp.Application.Queries.Application.Form;

public class GetApplicationFormByIdQuery : IRequest<BaseMediatrResponse<GetApplicationFormByIdQueryResponse>>
{
    public GetApplicationFormByIdQuery(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
    public Guid FormVersionId { get; set; }

}
