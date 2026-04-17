using MediatR;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using ReportModel = SFA.DAS.RecruitQa.Domain.Reports.Report;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;

public class GetReportsQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetReportsQuery, GetReportsQueryResult>
{
    public async Task<GetReportsQueryResult> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await recruitApiClient.Get<List<ReportModel>>(new GetReportsRequest());
        return new GetReportsQueryResult { Reports = reports ?? [] };
    }
}
