using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GetReports;

public class GetReportsQueryHandler(IRecruitApiClient recruitApiClient) : IRequestHandler<GetReportsQuery, GetReportsQueryResult>
{
    public async Task<GetReportsQueryResult> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await recruitApiClient.Get<List<Recruit.Contracts.ApiResponses.Report>>(new GetReportsApiRequest(ReportOwnerType.Qa));
        return new GetReportsQueryResult(reports ?? []);
    }
}