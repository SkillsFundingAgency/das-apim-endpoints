using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportById;
public class GetReportByIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetReportByIdQuery, GetReportByIdQueryResult>
{
    public async Task<GetReportByIdQueryResult> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<Domain.Reports.Report>(new GetReportByIdRequest(request.ReportId));
        return new GetReportByIdQueryResult(response);
    }
}