using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Sections;

public class GetAllSectionsQuery : IRequest<BaseMediatrResponse<GetAllSectionsQueryResponse>>
{
    public readonly Guid FormVersionId;

    public GetAllSectionsQuery(Guid formVersionId)
    {
        FormVersionId = formVersionId;
    }
}
