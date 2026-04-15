using MediatR;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.Domain.Reports;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;

public class GenerateQaReportQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, ICourseService courseService) : IRequestHandler<GenerateQaReportQuery, GenerateQaReportQueryResult>
{
    public async Task<GenerateQaReportQueryResult> Handle(GenerateQaReportQuery request, CancellationToken cancellationToken)
    {
        var reportDataTask = recruitApiClient.Get<GetGenerateQaReportResponse>(new GetGenerateQaReportRequest(request.ReportId));
        var reportTask = recruitApiClient.Get<Domain.Reports.Report>(new GetReportByIdRequest(request.ReportId));
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
