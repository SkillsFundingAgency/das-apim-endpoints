using SFA.DAS.RecruitQa.Application.Report.Queries.GenerateQaReport;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.Report;

public class WhenHandlingGenerateQaReportQuery
{
    [Test, MoqAutoData]
    public async Task Then_Report_Name_Is_Returned(
        Guid reportId,
        RecruitQa.Domain.Reports.Report report,
        GetStandardsListItem listItem,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICourseService> courseService,
        GenerateQaReportQueryHandler handler)
    {
        listItem.ApprenticeshipType = "Apprenticeship";
        listItem.Level = (int)ApprenticeshipLevel.Advanced;

        recruitApiClient
            .Setup(x => x.Get<GetGenerateQaReportResponse>(It.IsAny<GetGenerateQaReportRequest>()))
            .ReturnsAsync(new GetGenerateQaReportResponse { QaReports = [] });
        recruitApiClient
            .Setup(x => x.Get<RecruitQa.Domain.Reports.Report>(It.Is<GetReportByIdRequest>(r => r.GetUrl == $"api/reports/{reportId}")))
            .ReturnsAsync(report);
        courseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(new GetStandardsListResponse { Standards = [listItem] });

        var actual = await handler.Handle(new GenerateQaReportQuery(reportId), CancellationToken.None);

        actual.ReportName.Should().Be(report.Name);
    }

    [Test, MoqAutoData]
    public async Task Then_Reports_Are_Projected_With_Course_Name_And_Level(
        Guid reportId,
        RecruitQa.Domain.Reports.Report report,
        GetGenerateQaReportResponse apiResponse,
        GetStandardsListItem listItem,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICourseService> courseService,
        GenerateQaReportQueryHandler handler)
    {
        listItem.ApprenticeshipType = "Apprenticeship";
        listItem.Level = (int)ApprenticeshipLevel.Advanced;
        apiResponse.QaReports = [apiResponse.QaReports.First()];
        apiResponse.QaReports[0].ProgrammeId = listItem.LarsCode.ToString();

        recruitApiClient
            .Setup(x => x.Get<GetGenerateQaReportResponse>(
                It.Is<GetGenerateQaReportRequest>(r => r.GetUrl == $"api/reports/generate-qa/{reportId}")))
            .ReturnsAsync(apiResponse);
        recruitApiClient
            .Setup(x => x.Get<RecruitQa.Domain.Reports.Report>(It.IsAny<GetReportByIdRequest>()))
            .ReturnsAsync(report);
        courseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(new GetStandardsListResponse { Standards = [listItem] });

        var actual = await handler.Handle(new GenerateQaReportQuery(reportId), CancellationToken.None);

        actual.QaReports.Should().HaveCount(1);
        actual.QaReports[0].CourseName.Should().Be($"{listItem.LarsCode} {listItem.Title}");
        actual.QaReports[0].CourseLevel.Should().Be(ApprenticeshipLevel.Advanced.ToString());
    }

    [Test, MoqAutoData]
    public async Task Then_Empty_List_Returned_When_QaReports_Is_Empty(
        Guid reportId,
        RecruitQa.Domain.Reports.Report report,
        GetStandardsListItem listItem,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICourseService> courseService,
        GenerateQaReportQueryHandler handler)
    {
        listItem.ApprenticeshipType = "Apprenticeship";
        listItem.Level = (int)ApprenticeshipLevel.Intermediate;

        recruitApiClient
            .Setup(x => x.Get<GetGenerateQaReportResponse>(It.IsAny<GetGenerateQaReportRequest>()))
            .ReturnsAsync(new GetGenerateQaReportResponse { QaReports = [] });
        recruitApiClient
            .Setup(x => x.Get<RecruitQa.Domain.Reports.Report>(It.IsAny<GetReportByIdRequest>()))
            .ReturnsAsync(report);
        courseService
            .Setup(x => x.GetActiveStandards<GetStandardsListResponse>("ActiveStandards"))
            .ReturnsAsync(new GetStandardsListResponse { Standards = [listItem] });

        var actual = await handler.Handle(new GenerateQaReportQuery(reportId), CancellationToken.None);

        actual.QaReports.Should().BeEmpty();
    }

    private static void SetupMatchingProgrammeIds(GetGenerateQaReportResponse apiResponse, GetStandardsListResponse standardsResponse)
    {
        var standards = standardsResponse.Standards.ToList();
        apiResponse.QaReports = apiResponse.QaReports.Take(standards.Count).ToList();
        for (var i = 0; i < apiResponse.QaReports.Count; i++)
        {
            standards[i].ApprenticeshipType = "Apprenticeship";
            standards[i].Level = (int)ApprenticeshipLevel.Advanced;
            apiResponse.QaReports[i].ProgrammeId = standards[i].LarsCode.ToString();
        }
        standardsResponse.Standards = standards;
    }
}
