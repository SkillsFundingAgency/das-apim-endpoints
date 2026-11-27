using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.Domain.Reports;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;

public class GenerateQaReportQueryHandler(IRecruitApiClient recruitApiClient, ICourseService courseService) 
    : IRequestHandler<GenerateQaReportQuery, GenerateQaReportQueryResult>
{
    public async Task<GenerateQaReportQueryResult> Handle(GenerateQaReportQuery request, CancellationToken cancellationToken)
    {
        var reportDataTask = recruitApiClient.Get<GetQaReportResponse>(new GetReportsGenerateQaByReportIdApiRequest(request.ReportId));
        var reportTask = recruitApiClient.Get<Recruit.Contracts.ApiResponses.Report>(new GetReportsByReportIdApiRequest(request.ReportId));
        var standardsTask = courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        
        await Task.WhenAll(reportDataTask, standardsTask, reportTask);
        
        var standards = await standardsTask;
        var response = await reportDataTask;
        var report = await reportTask;
        var allTrainingProgrammes = standards.Standards?
            .Select(item => (TrainingProgramme)item).ToList() ?? [];

        var projectedQaReport = new List<QaReportProjected>();

        foreach (var qaReport in response.QaReports)
        {
            var programme = allTrainingProgrammes.FirstOrDefault(c => c.Id.Equals(qaReport.ProgrammeId, StringComparison.CurrentCultureIgnoreCase));
            projectedQaReport.Add(QaReportProjected.FromQaReport(qaReport, $"{programme?.Id} {programme?.Title}", programme?.ApprenticeshipLevel.ToString()));
        }
        
        return new GenerateQaReportQueryResult
        {
            QaReports = projectedQaReport ?? [],
            ReportName = report.Name
        };
    }
}
