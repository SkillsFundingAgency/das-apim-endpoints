using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportDataById;

public class GetReportDataByIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient)
    : IRequestHandler<GetReportDataByIdQuery, GetReportDataByIdQueryResult>
{
    public async Task<GetReportDataByIdQueryResult> Handle(GetReportDataByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<GetReportDataResponse>(new GetReportDataRequest(request.ReportId));
        return new GetReportDataByIdQueryResult
        {
            Reports = response?.Reports ?? []
        };
    }
}
