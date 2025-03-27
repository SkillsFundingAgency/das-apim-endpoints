using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsExportQuery : IRequest<BaseMediatrResponse<GetQualificationsExportResponse>>
    {
    }
}
