using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsQuery : IRequest<BaseMediatrResponse<GetNewQualificationsQueryResponse>>
    {
    }
}
