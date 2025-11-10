using SFA.DAS.Recruit.Application.Report.Query.GenerateReportsById;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using SFA.DAS.SharedOuterApi.Models;
using static SFA.DAS.Recruit.InnerApi.Recruit.Responses.Reports.GetGenerateReportResponse;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries;

[TestFixture]
internal class WhenHandlingGenerateReportByIdQuery
{
    [Test, MoqAutoData]
    public async Task Handle_WhenNoReportsExist_ReturnsEmptyList(
        Guid reportId,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GenerateReportsByReportIdQueryHandler handler)
    {
        // Arrange
        var query = new GenerateReportsByReportIdQuery(reportId);
        var recruitResponse = new GetGenerateReportResponse
        {
            ApplicationReviewReports = []
        };

        recruitApiClient
            .Setup(x => x.Get<GetGenerateReportResponse>(It.IsAny<GetGenerateReportRequest>()))
            .ReturnsAsync(recruitResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Reports.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenApplicationsExist_ShouldReturnMappedReports(
        Guid reportId,
        Guid applicationId,
        Guid candidateId,
        ApplicationReviewReport reviewReport,
        Recruit.Domain.Application application,
        GetStandardsListItemResponse courseResponse,
        Address address,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GenerateReportsByReportIdQueryHandler handler)
    {
        // Arrange
        var query = new GenerateReportsByReportIdQuery(reportId);
        reviewReport.ApplicationId = applicationId;
        application.Id = applicationId;
        reviewReport.CandidateId = candidateId;
        application.CandidateId = candidateId;
        application.Candidate = new Recruit.Domain.Candidate
        {
            FirstName = "John",
            LastName = "Doe"
        };
        application.EmploymentLocation = new Recruit.Domain.Location
        {
            Addresses =
            [
                new Recruit.Domain.EmploymentAddress
                {
                    IsSelected = true,
                    FullAddress = Newtonsoft.Json.JsonConvert.SerializeObject(address)
                }
            ]
        };

        recruitApiClient
            .Setup(x => x.Get<GetGenerateReportResponse>(It.IsAny<GetGenerateReportRequest>()))
            .ReturnsAsync(new GetGenerateReportResponse
            {
                ApplicationReviewReports = [reviewReport]
            });

        candidateApiClient.Setup(x =>
            x.Get<GetApplicationsByVacancyReferenceApiResponse>(
                It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()))
            .ReturnsAsync(new GetApplicationsByVacancyReferenceApiResponse
                {
                 Applications = [application]
                });

        coursesApiClientMock
            .Setup(x => x.Get<GetStandardsListItemResponse>(It.IsAny<GetStandardRequest>()))
            .ReturnsAsync(courseResponse);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Reports.Should().HaveCount(1);
        var report = result.Reports.First();
        report.Should().NotBeNull();
        report.ApplicationId.Should().Be(applicationId);
        report.CandidateName.Should().Be("John Doe");
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenCourseInfoNotFound_ShouldSkipApplication(Guid reportId,
        Guid applicationId,
        Guid candidateId,
        ApplicationReviewReport reviewReport,
        Recruit.Domain.Application application,
        GetStandardsListItemResponse courseResponse,
        Address address,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClientMock,
        GenerateReportsByReportIdQueryHandler handler)
    {
        // Arrange
        var query = new GenerateReportsByReportIdQuery(reportId);
        reviewReport.ApplicationId = applicationId;
        application.Id = applicationId;
        reviewReport.CandidateId = candidateId;
        application.CandidateId = candidateId;
        application.Candidate = new Recruit.Domain.Candidate
        {
            FirstName = "John",
            LastName = "Doe"
        };
        application.EmploymentLocation = new Recruit.Domain.Location
        {
            Addresses =
            [
                new Recruit.Domain.EmploymentAddress
                {
                    IsSelected = true,
                    FullAddress = Newtonsoft.Json.JsonConvert.SerializeObject(address)
                }
            ]
        };

        recruitApiClient
            .Setup(x => x.Get<GetGenerateReportResponse>(It.IsAny<GetGenerateReportRequest>()))
            .ReturnsAsync(new GetGenerateReportResponse
            {
                ApplicationReviewReports = [reviewReport]
            });

        candidateApiClient.Setup(x =>
                x.Get<GetApplicationsByVacancyReferenceApiResponse>(
                    It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()))
            .ReturnsAsync(new GetApplicationsByVacancyReferenceApiResponse
            {
                Applications = [application]
            });

        coursesApiClientMock
            .Setup(x => x.Get<GetStandardsListItemResponse>(It.IsAny<GetStandardRequest>()))
            .ReturnsAsync((GetStandardsListItemResponse)null!);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Reports.Should().BeEmpty();
    }
}