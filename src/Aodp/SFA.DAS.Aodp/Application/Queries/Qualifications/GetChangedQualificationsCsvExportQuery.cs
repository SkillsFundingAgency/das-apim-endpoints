using MediatR;

namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsCsvExportQuery : IRequest<BaseMediatrResponse<GetChangedQualificationsCsvExportResponse>>
    {
    }
}
