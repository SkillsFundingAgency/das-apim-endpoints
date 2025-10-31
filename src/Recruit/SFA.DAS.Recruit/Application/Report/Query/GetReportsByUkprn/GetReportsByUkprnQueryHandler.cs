using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Report.Query.GetReportsByUkprn;
public class GetReportsByUkprnQueryHandler(IRecruitApiClient<RecruitApiConfiguration> apiClient) : IRequestHandler<GetReportsByUkprnQuery, GetReportsByUkprnQueryResult>
{
    public async Task<GetReportsByUkprnQueryResult> Handle(GetReportsByUkprnQuery request, CancellationToken cancellationToken)
    {
        var response = await apiClient.Get<List<Domain.Reports.Report>>(new GetReportsByUkprnRequest(request.Ukprn));
        return new GetReportsByUkprnQueryResult(response);
    }
}